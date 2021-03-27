import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ScheduledMessage } from 'src/app/models/scheduledmessage.model';
import { DeleteMessageDialogComponent } from './dialogs/delete-message-dialog/delete-message-dialog.component';
import { DeleteOptionDialogComponent } from './dialogs/delete-option-dialog/delete-option-dialog.component';
import { LightModeService } from 'src/app/services/light-mode/light-mode.service';

@Component({
    selector: 'app-message',
    templateUrl: './message.component.html',
    styleUrls: ['./message.component.scss']
})
export class MessageComponent implements OnInit {

    @Input() message: ScheduledMessage;
    @Output() deleteMessage: EventEmitter<string>;
    @Output() deleteOption: EventEmitter<{ id: string, index: number }>;
    @Output() addMessage: EventEmitter<{ id: string, message: string }>;
    @Output() updateMessage: EventEmitter<ScheduledMessage>;

    messageForm: FormGroup;
    lightMode: boolean;

    constructor(public dialog: MatDialog, private lightModeService: LightModeService, private fb: FormBuilder) {
        this.lightModeService.$lightMode.subscribe(lightMode => this.lightMode = lightMode);

        this.deleteMessage = new EventEmitter();
        this.deleteOption = new EventEmitter();
        this.addMessage = new EventEmitter();
        this.updateMessage = new EventEmitter();
    }

    ngOnInit(): void {
        this.messageForm = this.fb.group({
            channel: [this.message.channelId, [Validators.pattern("^[0-9]+$")]],
            schedule: this.message.timeSchedule,
            messages: this.fb.array(this.message.messages)
        });
    }

    addMessageControl(): void {
        this.messages.push(this.fb.control(''));
    }

    removeOption(index: number): void {
        const dialogRef = this.dialog.open(DeleteOptionDialogComponent, {
            data: {
                schedule: this.message.timeSchedule,
                message: this.message.messages[index],
                lightMode: this.lightMode
            },
            panelClass: this.lightMode ? '' : 'darkMode'
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.deleteOption.emit({ id: this.message.id, index: index });
                this.messages.removeAt(index);
            }
        });
    }

    removeMessage(): void {
        const dialogRef = this.dialog.open(DeleteMessageDialogComponent, {
            data: {
                channel: this.message.channelId,
                schedule: this.message.timeSchedule,
                lightMode: this.lightMode
            },
            panelClass: this.lightMode ? '' : 'darkMode'
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.deleteMessage.emit(this.message.id);
            }
        });
    }

    submitUpdateMessage(): void {
        this.message.channelId = this.messageForm.value.channel as number;
        this.message.timeSchedule = this.messageForm.value.schedule;
        this.updateMessage.emit(this.message);
    }

    get messages(): FormArray {
        return this.messageForm.get('messages') as FormArray;
    }

    get formChanged(): boolean {
        const channel = this.messageForm.get('channel');
        const schedule = this.messageForm.get('schedule');
        return (channel.dirty && (channel.value as number) != this.message.channelId)
            || (schedule.dirty && schedule.value != this.message.timeSchedule);
    }

}
