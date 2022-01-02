import { Component, Injector } from '@angular/core';
import { TuiDialogService } from '@taiga-ui/core';
import { first } from 'rxjs';
import { MessageFilter } from '../../models/message-filter.model';
import { ScheduledMessage } from '../../models/scheduled-message.model';
import { MessagesService } from '../../services/messages.service';
import { PolymorpheusComponent } from '@tinkoff/ng-polymorpheus';
import { MessageDialogComponent } from '../message-dialog/message-dialog.component';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss']
})
export class MessagesComponent {

  filter: MessageFilter = new MessageFilter();
  messages: ScheduledMessage[];
  loaded = false;

  constructor(private service: MessagesService, private dialogService: TuiDialogService, private injector: Injector) {
    this.messages = [];
    service.$messages.pipe(first()).subscribe(_ => this.loaded = true);
    service.$messages.subscribe(messages => this.messages = messages);
    this.service.getMessages();
  }

  get filteredMessages(): ScheduledMessage[] {
    return this.messages
      .filter(msg => this.filter.channel == '' || msg.channelId.toString().includes(this.filter.channel))
      .filter(msg => this.filter.schedule == '' || msg.timeSchedule.includes(this.filter.schedule))
      .filter(msg => this.filter.description == ''
        || msg.description.includes(this.filter.description)
        || msg.messages.some(m => m.includes(this.filter.description)));
  }

  clearFilters(): void {
    this.filter = new MessageFilter();
  }

  public openDialog(): void {
    this.dialogService.open(new PolymorpheusComponent(MessageDialogComponent, this.injector), { size: 'm' }).subscribe();
  }

}
