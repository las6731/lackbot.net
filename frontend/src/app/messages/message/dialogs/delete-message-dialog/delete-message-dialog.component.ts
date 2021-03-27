import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
    selector: 'app-delete-message-dialog',
    templateUrl: './delete-message-dialog.component.html'
})
export class DeleteMessageDialogComponent {

    constructor(@Inject(MAT_DIALOG_DATA) public data: DeleteMessageDialogData) { }

}

export interface DeleteMessageDialogData {
    channel: number;
    schedule: string;
    lightMode: boolean;
}
