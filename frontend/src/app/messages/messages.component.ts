import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ScheduledMessage } from '../models/scheduledmessage.model';
import { LightModeService } from '../services/light-mode/light-mode.service';
import { ScheduledMessagesService } from '../services/scheduled-messages/scheduled-messages.service';

@Component({
    selector: 'app-messages',
    templateUrl: './messages.component.html',
    styleUrls: ['./messages.component.scss']
})
export class MessagesComponent {

    messages: ScheduledMessage[];
    lightMode: boolean;
    addMessageForm: FormGroup;

    constructor(private messagesService: ScheduledMessagesService, private lightModeService: LightModeService, private fb: FormBuilder) {
        this.lightModeService.$lightMode.subscribe(lightMode => this.lightMode = lightMode);
        this.messagesService.$messages.subscribe(messages => this.messages = messages);
        this.messagesService.getMessages();

        this.addMessageForm = fb.group({
            channel: ['', [Validators.pattern("^[0-9]+$")]],
            timeSchedule: '',
            message: ''
        });
    }

    addScheduledMessage(): void {
        const channel = this.addMessageForm.value.channel as number;
        const schedule = this.addMessageForm.value.timeSchedule.trim();
        const message = this.addMessageForm.value.message.trim();

        let scheduledMessage = new ScheduledMessage(channel, schedule, [message]);

        this.messagesService.addScheduledMessage(scheduledMessage);

        this.addMessageForm.reset();
    }

    addMessage(event: { id: string, message: string }): void {
        event.message = event.message.trim();
        this.messagesService.addMessage(event.id, event.message);
    }

    updateMessage(event: { id: string, index: number, message: string }): void {
        event.message = event.message.trim();
        this.messagesService.updateMessage(event.id, event.index, event.message);
    }

    updateScheduledMessage(event: ScheduledMessage): void {
        this.messagesService.updateScheduledMessage(event);
    }

    removeMessage(event: { id: string, index: number }): void {
        this.messagesService.deleteMessage(event.id, event.index);
    }

    removeScheduledMessage(id: string): void {
        this.messagesService.deleteScheduledMessage(id);
    }
}
