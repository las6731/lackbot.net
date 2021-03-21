import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormArray, FormGroup } from '@angular/forms';
import { AutoResponse, AutoResponseType } from 'src/app/models/autoresponse.model';

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

    typeOptions = Object.values(AutoResponseType);

    constructor(private fb: FormBuilder) {
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
        this.deleteResponse.emit({ id: this.autoResponse.id, index: index });
        this.responses.removeAt(index);
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
