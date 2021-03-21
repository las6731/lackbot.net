import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AutoResponse, AutoResponseType } from '../models/autoresponse.model';
import { LightModeService } from '../services/light-mode/light-mode.service';
import { ResponseService } from '../services/response/response.service';

@Component({
    selector: 'app-responses',
    templateUrl: './responses.component.html',
    styleUrls: ['./responses.component.scss']
})
export class ResponsesComponent {

    responses: AutoResponse[];
    lightMode: boolean;
    addPhraseForm: FormGroup;

    constructor(private responseService: ResponseService, private lightModeService: LightModeService, private fb: FormBuilder) {
        this.lightModeService.$lightMode.subscribe(lightMode => this.lightMode = lightMode);
        this.responseService.$responses.subscribe(responses => this.responses = responses);
        this.responseService.getResponses();

        this.addPhraseForm = fb.group({
            phrase: '',
            response: ''
        });
    }

    addResponse(event: { id: string, response: string }): void {
        event.response = event.response.trim();
        this.responseService.addResponse(event.id, event.response);
    }

    replaceResponse(event: AutoResponse): void {
        this.responseService.replaceResponse(event);
    }

    removeResponse(event: { id: string, index: number }): void {
        if (event.index === undefined) {
            return;
        }
        this.responseService.deleteResponse(event.id, event.index);
    }

    removePhrase(id: string): void {
        this.responseService.deleteAutoResponse(id);
    }

    addPhrase(): void {
        const phrase = this.addPhraseForm.value.phrase.trim().toLowerCase();
        const response = this.addPhraseForm.value.response.trim();

        let autoResponse = new AutoResponse(phrase, [response]);

        this.responseService.addAutoResponse(autoResponse);

        this.addPhraseForm.reset();
    }

}
