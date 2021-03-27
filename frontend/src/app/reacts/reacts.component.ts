import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AutoReact, AutoReactType } from '../models/autoreact.model';
import { LightModeService } from '../services/light-mode/light-mode.service';
import { ReactService } from '../services/react/react.service';

@Component({
    selector: 'app-reacts',
    templateUrl: './reacts.component.html',
    styleUrls: ['./reacts.component.scss']
})
export class ReactsComponent {

    reacts: AutoReact[];
    lightMode: boolean;
    addReactionForm: FormGroup;

    typeOptions = Object.values(AutoReactType);

    constructor(private reactService: ReactService, private lightModeService: LightModeService, private fb: FormBuilder) {
        this.lightModeService.$lightMode.subscribe(lightMode => this.lightMode = lightMode);
        this.reactService.$reacts.subscribe(reacts => this.reacts = reacts);
        this.reactService.getReacts();

        this.addReactionForm = fb.group({
            phrase: '',
            type: AutoReactType.Naive,
            authorId: ['', [Validators.pattern("^[0-9]+$")]],
            emoji: ''
        });
    }

    addAutoReact(): void {
        let phrase = this.addReactionForm.value.phrase;
        if (phrase == null) {
            phrase = '';
        } else {
            phrase = phrase.trim().toLowerCase();
        }

        const emoji = this.addReactionForm.value.emoji.trim();
        const type = this.addReactionForm.value.type as AutoReactType;

        let autoReact = new AutoReact(phrase, emoji);
        autoReact.type = type;

        const authorId = this.addReactionForm.value.authorId;
        if (authorId != '') {
            autoReact.author = authorId as number;
        }

        this.reactService.addAutoReact(autoReact);

        this.addReactionForm.reset();
    }

    updateEmoji(event: { id: string, emoji: string }): void {
        event.emoji = event.emoji.trim();
        this.reactService.updateEmoji(event.id, event.emoji);
    }

    replaceReact(event: AutoReact): void {
        this.reactService.replaceReact(event);
    }

    removeReact(id: string): void {
        this.reactService.deleteReact(id);
    }

    get showAuthor(): boolean {
        let form = this.addReactionForm.get('type').value;
        return form == AutoReactType.Author;
    }

    get formValid(): boolean {
        return (this.addReactionForm.get('phrase').valid || (this.showAuthor && this.addReactionForm.get('authorId').valid))
            && this.addReactionForm.get('emoji').valid;
    }
}
