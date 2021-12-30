import { Component, Input, OnInit } from '@angular/core';
import { AutoResponse, AutoResponseType } from '../models/autoresponse.model';
import * as util from 'src/util/util';
import { TuiDialogContext, TuiDialogService } from '@taiga-ui/core';
import { PolymorpheusContent } from '@tinkoff/ng-polymorpheus';
import { faClock, faCode, faDumbbell } from '@fortawesome/free-solid-svg-icons';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-response',
  templateUrl: './response.component.html',
  styleUrls: ['./response.component.scss']
})
export class ResponseComponent implements OnInit {

  @Input() response!: AutoResponse

  strongIcon = faDumbbell;
  timeIcon = faClock;
  regexIcon = faCode;

  typeOptions = Object.keys(AutoResponseType).map(type => type as AutoResponseType);

  form: FormGroup;

  constructor(private dialogService: TuiDialogService, private fb: FormBuilder) {
    this.form = fb.group({
      description: fb.control(''),
      phrase: fb.control(''),
      type: fb.control(''),
      responses: fb.array(['']),
      timeSchedule: fb.control('')
    });
  }

  ngOnInit(): void {
    this.form.reset();
    this.form.controls['description'].setValue(this.response.description);
    this.form.controls['phrase'].setValue(this.response.phrase);
    this.form.controls['type'].setValue(this.response.type);
    this.form.setControl('responses', this.fb.array(this.response.responses));
    this.form.controls['timeSchedule'].setValue(this.response.timeSchedule);
    this.form.disable();
  }

  get typeDisplay(): string {
    return util.forDisplay(this.response.type);
  }

  get responseDisplay(): string {
    if (this.response.responses.length == 1) {
      if (this.response.responses[0].length < 28) return this.response.responses[0];
      return this.response.responses[0].substring(0, 26) + '...';
    }
    return `${this.response.responses.length} responses`;
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

  public typeForDisplay(type: AutoResponseType): string {
    return util.forDisplay(type);
  }

  public openDialog(content: PolymorpheusContent<TuiDialogContext>, header: PolymorpheusContent): void {
    this.dialogService.open(content, {
      header: header,
      size: 'm'
    }).pipe(finalize(() => this.ngOnInit())) // reset form when dialog is closed
    .subscribe();
  }

  public edit(): void {
    if (this.form.disabled) this.form.enable();
  }

  public save(): void {
    this.form.disable();
    // todo: save
  }

  public addResponse(): void {
    this.responsesControl.push(this.fb.control(''));
  }

  public responseChanged(i: number): void {
    let val = this.responsesControl.controls[i].value;
    if (val == '') this.responsesControl.removeAt(i);
  }

}
