import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { ResponsesComponent } from './responses/responses.component';
import { ResponseService } from './services/response/response.service';
import { ReactService } from './services/react/react.service';
import { ScheduledMessagesService } from './services/scheduled-messages/scheduled-messages.service';
import { HttpClientModule } from '@angular/common/http';
import { LightModeService } from './services/light-mode/light-mode.service';
import { PhraseComponent } from './responses/phrase/phrase.component';
import { DeleteResponseDialogComponent } from './responses/phrase/dialogs/delete-response-dialog/delete-response-dialog.component';
import { DeletePhraseDialogComponent } from './responses/phrase/dialogs/delete-phrase-dialog/delete-phrase-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { ReactsComponent } from './reacts/reacts.component';
import { ReactionComponent } from './reacts/reaction/reaction.component';
import { MessagesComponent } from './messages/messages.component';
import { MessageComponent } from './messages/message/message.component';
import { DeleteOptionDialogComponent } from './messages/message/dialogs/delete-option-dialog/delete-option-dialog.component';
import { DeleteMessageDialogComponent } from './messages/message/dialogs/delete-message-dialog/delete-message-dialog.component';

@NgModule({
    declarations: [
        AppComponent,
        ResponsesComponent,
        PhraseComponent,
        DeleteResponseDialogComponent,
        DeletePhraseDialogComponent,
        ReactsComponent,
        ReactionComponent,
        MessagesComponent,
        MessageComponent,
        DeleteOptionDialogComponent,
        DeleteMessageDialogComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        NgbModule,
        FormsModule,
        ReactiveFormsModule,
        BrowserAnimationsModule,
        MatSlideToggleModule,
        MatDialogModule,
        MatButtonModule,
        HttpClientModule
    ],
    providers: [
        ResponseService,
        LightModeService,
        ReactService,
        ScheduledMessagesService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
