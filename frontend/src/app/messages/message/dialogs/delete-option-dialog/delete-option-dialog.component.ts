import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
    selector: 'app-delete-option-dialog',
    templateUrl: './delete-option-dialog.component.html'
})
export class DeleteOptionDialogComponent {

    constructor(@Inject(MAT_DIALOG_DATA) public data: DeleteOptionDialogData) { }

}

export interface DeleteOptionDialogData {
    schedule: string;
    message: string;
    lightMode: boolean;
}
