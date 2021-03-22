import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
    selector: 'app-delete-phrase-dialog',
    templateUrl: './delete-phrase-dialog.component.html'
})
export class DeletePhraseDialogComponent {

    constructor(@Inject(MAT_DIALOG_DATA) public data: DeletePhraseDialogData) { }

}

export interface DeletePhraseDialogData {
    phrase: string;
    lightMode: boolean;
}
