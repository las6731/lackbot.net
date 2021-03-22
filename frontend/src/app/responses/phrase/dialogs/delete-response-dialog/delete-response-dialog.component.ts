import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
    selector: 'app-delete-response-dialog',
    templateUrl: './delete-response-dialog.component.html'
})
export class DeleteResponseDialogComponent {

    constructor(@Inject(MAT_DIALOG_DATA) public data: DeleteResponseDialogData) { }

}

export interface DeleteResponseDialogData {
    phrase: string;
    response: string;
    lightMode: boolean;
}
