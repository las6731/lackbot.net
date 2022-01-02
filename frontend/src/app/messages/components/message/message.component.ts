import { Component, Injector, Input } from '@angular/core';
import { TuiDialogService } from '@taiga-ui/core';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { ScheduledMessage } from '../../models/scheduled-message.model';
import { MessageDialogComponent } from '../message-dialog/message-dialog.component';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.scss']
})
export class MessageComponent {

  @Input() message!: ScheduledMessage

  constructor(private dialogService: TuiDialogService, private injector: Injector) { }

  get messageDisplay(): string {
    if (this.message.messages.length == 1) {
      if (this.message.messages[0].length < 28) return this.message.messages[0];
      return this.message.messages[0].substring(0, 26) + '...';
    }
    return `${this.message.messages.length} messages`;
  }

  public openDialog(): void {
    this.dialogService.open(new PolymorpheusComponent(MessageDialogComponent, this.injector), {
      size: 'm',
      data: this.message
    }).subscribe();
  }

}
