import { ChangeDetectorRef, Component, Inject, Injector } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { TuiDialogService, TuiDialogContext } from '@taiga-ui/core';
import { POLYMORPHEUS_CONTEXT, PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { first } from 'rxjs';
import { ConfirmationDialogComponent } from 'src/app/shared/components/confirmation-dialog/confirmation-dialog.component';
import { ScheduledMessage } from '../../models/scheduled-message.model';
import { MessagesService } from '../../services/messages.service';

@Component({
  selector: 'app-message-dialog',
  templateUrl: './message-dialog.component.html',
  styleUrls: ['./message-dialog.component.scss']
})
export class MessageDialogComponent {

  form: FormGroup;
  value: ScheduledMessage | null = null;
  isEdit = true;
  loading = false;
  deleting = false;

  constructor(private fb: FormBuilder, private service: MessagesService,
    private dialogService: TuiDialogService, private injector: Injector, private changeDetector: ChangeDetectorRef,
    @Inject(POLYMORPHEUS_CONTEXT) public context: TuiDialogContext<ScheduledMessage | null, ScheduledMessage | null>) {
    if (this.context.data == null) {
      this.isEdit = false;
    }

    this.form = fb.group({
      description: fb.control({ value: this.context.data?.description ?? '', disabled: true}),
      timeSchedule: fb.control(this.context.data?.timeSchedule ?? '', Validators.required),
      channelId: fb.control(this.context.data?.channelId ?? '', Validators.required),
      messages: fb.array(this.context.data?.messages ?? [''], Validators.required)
    });

    if (this.isEdit) this.form.disable();
  }

  get messagesControl(): FormArray {
    return this.form.controls['messages'] as FormArray;
  }

  get canSave(): boolean {
    return this.form.valid && this.form.dirty && this.form.enabled;
  }

  public edit(): void {
    if (this.form.disabled) this.form.enable();
  }

  public save(): void {
    this.form.disable();
    this.loading = true;

    this.service.$messages.pipe(first()).subscribe(() => {
      this.loading = false;
      if (!this.isEdit) this.context.completeWith(null);
      this.changeDetector.detectChanges();
    });

    let message = this.form.value as ScheduledMessage;
    if (this.isEdit) {
      message.id = this.context.data?.id;
      this.service.updateScheduledMessage(message);
    } else {
      this.service.addScheduledMessage(message);
    }
  }

  public close(): void {
    this.context.completeWith(null);
  }

  public addMessage(): void {
    this.messagesControl.push(this.fb.control(''));
  }

  public deleteMessage(): void {

    this.dialogService.open(new PolymorpheusComponent(ConfirmationDialogComponent, this.injector),
    {
      label: 'Confirm Deletion',
      data: 'Are you sure you want to delete this message?',
      size: 's'
    }).subscribe(confirm => {
      if (confirm == true) {
        this.deleting = true;
        this.service.$messages.pipe(first()).subscribe(() => this.close());
        this.service.deleteScheduledMessage(this.context.data?.id!);
      }
    });
  }

  public messageChanged(event: any, i: number): void {
    if (event == '') this.messagesControl.removeAt(i);
  }

}
