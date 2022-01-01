import { ChangeDetectorRef, Component, Inject, Injector } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { faDumbbell, faClock, faCode } from '@fortawesome/free-solid-svg-icons';
import { AutoResponse, AutoResponseType } from '../../models/autoresponse.model';
import * as util from 'src/util/util';
import { TuiDialogContext, TuiDialogService } from '@taiga-ui/core';
import { POLYMORPHEUS_CONTEXT, PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { ResponsesService } from '../../services/responses.service';
import { first } from 'rxjs';
import { ConfirmationDialogComponent } from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';

@Component({
  selector: 'app-response-dialog',
  templateUrl: './response-dialog.component.html',
  styleUrls: ['./response-dialog.component.scss']
})
export class ResponseDialogComponent {

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
    private dialogService: TuiDialogService, private injector: Injector, private changeDetector: ChangeDetectorRef,
    @Inject(POLYMORPHEUS_CONTEXT) public context: TuiDialogContext<AutoResponse | null, AutoResponse | null>) {
    if (this.context.data == null) {
      this.isEdit = false;
    }

    this.form = fb.group({
      description: fb.control({ value: this.context.data?.description ?? '', disabled: true}),
      phrase: fb.control(this.context.data?.phrase ?? '', Validators.required),
      type: fb.control(this.context.data?.type ?? AutoResponseType.Naive, Validators.required),
      responses: fb.array(this.context.data?.responses ?? [''], Validators.required),
      timeSchedule: fb.control(this.context.data?.timeSchedule ?? '')
    });

    if (this.isEdit) this.form.disable();
  }

  public typeForDisplay(type: AutoResponseType): string {
    return util.responseTypeForDisplay(type);
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

  get canSave(): boolean {
    if (this.form.invalid || this.form.pristine || this.form.disabled) return false;
    if (this.form.controls['type'].value == AutoResponseType.TimeBasedYesNo) {
      if (this.responsesControl.length != 2) return false;
    }
    return true;
  }

  public edit(): void {
    if (this.form.disabled) this.form.enable();
  }

  public save(): void {
    this.form.disable();
    this.loading = true;

    this.service.$responses.pipe(first()).subscribe(() => {
      this.loading = false;
      if (!this.isEdit) this.context.completeWith(null);
      this.changeDetector.detectChanges();
    });

    let response = this.form.value as AutoResponse;
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

    this.dialogService.open(new PolymorpheusComponent(ConfirmationDialogComponent, this.injector),
    {
      label: 'Confirm Deletion',
      data: 'Are you sure you want to delete this response?',
      size: 's'
    }).subscribe(confirm => {
      if (confirm == true) {
        this.deleting = true;
        this.service.$responses.pipe(first()).subscribe(() => this.close());
        this.service.deleteAutoResponse(this.context.data?.id!);
      }
    });
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
