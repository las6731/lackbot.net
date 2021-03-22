import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormArray, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { AutoResponse, AutoResponseType } from 'src/app/models/autoresponse.model';
import { LightModeService } from 'src/app/services/light-mode/light-mode.service';
import { DeletePhraseDialogComponent } from './dialogs/delete-phrase-dialog/delete-phrase-dialog.component';
import { DeleteResponseDialogComponent } from './dialogs/delete-response-dialog/delete-response-dialog.component';

@Component({
    selector: 'app-phrase',
    templateUrl: './phrase.component.html',
    styleUrls: ['./phrase.component.scss']
})
export class PhraseComponent implements OnInit {

    @Input() autoResponse: AutoResponse;
    @Output() deletePhrase: EventEmitter<string>;
    @Output() deleteResponse: EventEmitter<{ id: string, index: number }>;
    @Output() addResponse: EventEmitter<{ id: string, response: string }>;
    @Output() replaceResponse: EventEmitter<AutoResponse>;

    phraseForm: FormGroup;
    lightMode: boolean;
    typeOptions = Object.values(AutoResponseType);

    constructor(public dialog: MatDialog, private lightModeService: LightModeService, private fb: FormBuilder) {
        this.lightModeService.$lightMode.subscribe(lightMode => this.lightMode = lightMode);

        this.deletePhrase = new EventEmitter();
        this.deleteResponse = new EventEmitter();
        this.addResponse = new EventEmitter();
        this.replaceResponse = new EventEmitter();
    }

    ngOnInit(): void {
        this.phraseForm = this.fb.group({
            phrase: this.autoResponse.phrase,
            type: this.autoResponse.type,
            timeSchedule: this.autoResponse.timeSchedule,
            responses: this.fb.array(this.autoResponse.responses)
        });
    }

    addResponseControl(): void {
        this.responses.push(this.fb.control(''));
    }

    removeResponse(index: number): void {
        const dialogRef = this.dialog.open(DeleteResponseDialogComponent, {
            data: {
                phrase: this.autoResponse.phrase,
                response: this.autoResponse.responses[index],
                lightMode: this.lightMode
            },
            panelClass: this.lightMode ? '' : 'darkMode'
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.deleteResponse.emit({ id: this.autoResponse.id, index: index });
                this.responses.removeAt(index);
            }
        });
    }

    removePhrase(): void {
        const dialogRef = this.dialog.open(DeletePhraseDialogComponent, {
            data: {
                phrase: this.autoResponse.phrase,
                lightMode: this.lightMode
            },
            panelClass: this.lightMode ? '' : 'darkMode'
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.deletePhrase.emit(this.autoResponse.id);
            }
        });
    }

    submitReplaceResponse(): void {
        this.autoResponse.type = this.phraseForm.get('type').value;
        this.autoResponse.timeSchedule = this.phraseForm.get('timeSchedule').value
        this.replaceResponse.emit(this.autoResponse);
    }

    get responses(): FormArray {
        return this.phraseForm.get('responses') as FormArray;
    }

    get typeChanged(): boolean {
        let form = this.phraseForm.get('type');
        return form.dirty && form.value != this.autoResponse.type;
    }

    get showSchedule(): boolean {
        let form = this.phraseForm.get('type').value;
        return form == AutoResponseType.TimeBased || form == AutoResponseType.TimeBasedYesNo;
    }

    get scheduleChanged(): boolean {
        let form = this.phraseForm.get('timeSchedule');
        return this.showSchedule && form.dirty && form.value != this.autoResponse.timeSchedule;
    }

}
