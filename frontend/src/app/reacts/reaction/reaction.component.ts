import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { AutoReact, AutoReactType } from 'src/app/models/autoreact.model';
import { DeletePhraseDialogComponent } from 'src/app/responses/phrase/dialogs/delete-phrase-dialog/delete-phrase-dialog.component';
import { LightModeService } from 'src/app/services/light-mode/light-mode.service';

@Component({
    selector: 'app-reaction',
    templateUrl: './reaction.component.html',
    styleUrls: ['./reaction.component.scss']
})
export class ReactionComponent implements OnInit {

    @Input() autoReact: AutoReact;
    @Output() deleteReaction: EventEmitter<string>;
    @Output() updateEmoji: EventEmitter<{ id: string, emoji: string }>;
    @Output() replaceReaction: EventEmitter<AutoReact>;

    reactionForm: FormGroup;
    lightMode: boolean;
    typeOptions = Object.values(AutoReactType);

    constructor(public dialog: MatDialog, private lightModeService: LightModeService, private fb: FormBuilder) {
        this.lightModeService.$lightMode.subscribe(lightMode => this.lightMode = lightMode);

        this.deleteReaction = new EventEmitter();
        this.updateEmoji = new EventEmitter();
        this.replaceReaction = new EventEmitter();
    }

    ngOnInit(): void {
        this.reactionForm = this.fb.group({
            phrase: this.autoReact.phrase,
            type: this.autoReact.type,
            authorId: this.autoReact.author,
            emoji: this.autoReact.emoji
        });
    }

    removeReaction(): void {
        const dialogRef = this.dialog.open(DeletePhraseDialogComponent, {
            data: {
                phrase: this.autoReact.phrase,
                lightMode: this.lightMode
            },
            panelClass: this.lightMode ? '' : 'darkMode'
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.deleteReaction.emit(this.autoReact.id);
            }
        });
    }

    submitReplaceReaction(): void {
        this.autoReact.type = this.reactionForm.get('type').value;
        this.autoReact.author = this.reactionForm.get('authorId').value;
        this.replaceReaction.emit(this.autoReact);
    }

    get emoji(): FormControl {
        return this.reactionForm.get('emoji') as FormControl;
    }

    get typeChanged(): boolean {
        let form = this.reactionForm.get('type');
        return form.dirty && form.value != this.autoReact.type;
    }

    get showAuthor(): boolean {
        let form = this.reactionForm.get('type').value;
        return form == AutoReactType.Author;
    }

    get authorChanged(): boolean {
        let form = this.reactionForm.get('authorId');
        return this.showAuthor && form.dirty && form.value != this.autoReact.author;
    }

}
