import { Component, Inject } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { faDumbbell, faClock, faCode } from '@fortawesome/free-solid-svg-icons';
import { AutoResponse, AutoResponseType } from '../models/autoresponse.model';
import * as util from '../../../util/util';
import { TuiDialogContext } from '@taiga-ui/core';
import {POLYMORPHEUS_CONTEXT} from '@tinkoff/ng-polymorpheus';
import { ResponsesService } from '../services/responses.service';
import { first } from 'rxjs';

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.scss']
})
export class DialogComponent {

  strongIcon = faDumbbell;
  timeIcon = faClock;
  regexIcon = faCode;

  typeOptions = Object.keys(AutoResponseType).map(type => type as AutoResponseType);

  form: FormGroup;
  value: AutoResponse | null = null;
  isEdit = true;
  loading = false;
  deleting = false;

  constructor(private fb: FormBuilder, private service: ResponsesService,
    @Inject(POLYMORPHEUS_CONTEXT) public context: TuiDialogContext<AutoResponse | null, AutoResponse | null>) {
    if (this.context.data == null) {
      this.isEdit = false;
    }

    this.form = fb.group({
      description: fb.control(this.context.data?.description ?? ''),
      phrase: fb.control(this.context.data?.phrase ?? '', Validators.required),
      type: fb.control(this.context.data?.type ?? AutoResponseType.Naive, Validators.required),
      responses: fb.array(this.context.data?.responses ?? [''], Validators.required),
      timeSchedule: fb.control(this.context.data?.timeSchedule ?? '')
    });

    if (this.isEdit) this.form.disable();
  }

  public typeForDisplay(type: AutoResponseType): string {
    return util.forDisplay(type);
  }

  get responsesControl(): FormArray {
    return this.form.controls['responses'] as FormArray;
  }

  get isTimeBased(): boolean {
    return util.isTimeBased(this.form.controls['type'].value);
  }

  get isRegex(): boolean {
    return this.form.controls['type'].value == AutoResponseType.Regex;
  }

  get isStrong(): boolean {
    return this.form.controls['type'].value == AutoResponseType.Strong;
  }

  public edit(): void {
    if (this.form.disabled) this.form.enable();
  }

  public save(): void {
    this.form.disable();
    this.loading = true;
    
    let response = this.form.value as AutoResponse;

    this.service.$responses.pipe(first()).subscribe(() => {
      this.loading = false;
      if (!this.isEdit) this.context.completeWith(null);
    });

    if (this.isEdit) {
      response.id = this.context.data?.id;
      this.service.updateResponse(response);
    } else {
      this.service.addAutoResponse(response);
    }
  }

  public close(): void {
    this.context.completeWith(null);
  }

  public addResponse(): void {
    this.responsesControl.push(this.fb.control(''));
  }

  public deleteResponse(): void {
    this.deleting = true;

    this.service.$responses.pipe(first()).subscribe(() => this.close());

    this.service.deleteAutoResponse(this.context.data?.id!);
  }

  public responseChanged(event: any, i: number): void {
    if (event == '') this.responsesControl.removeAt(i);
  }

  public typeChanged(): void {
    if (!this.isTimeBased && this.form.controls['timeSchedule'].validator != null) {
      this.form.controls['timeSchedule'].clearValidators();
      this.form.controls['timeSchedule'].setValue('');
    }
  }
}
