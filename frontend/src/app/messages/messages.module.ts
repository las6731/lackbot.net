import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MessagesRoutingModule } from './messages-routing.module';
import { MessagesComponent } from './components/messages/messages.component';
import { MessageComponent } from './components/message/message.component';
import { MessageDialogComponent } from './components/message-dialog/message-dialog.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TuiButtonModule, TuiDataListModule, TuiLabelModule, TuiTextfieldControllerModule, TuiLoaderModule, TuiTooltipModule } from '@taiga-ui/core';
import { TuiIslandModule, TuiInputModule, TuiMultiSelectModule, TuiTextAreaModule } from '@taiga-ui/kit';
import { SharedModule } from '../shared/shared.module';
import { MessagesService } from './services/messages.service';


@NgModule({
  declarations: [
    MessagesComponent,
    MessageComponent,
    MessageDialogComponent
  ],
  imports: [
    CommonModule,
    MessagesRoutingModule,
    TuiIslandModule,
    TuiInputModule,
    TuiButtonModule,
    TuiMultiSelectModule,
    TuiDataListModule,
    TuiLabelModule,
    TuiTextfieldControllerModule,
    TuiTextAreaModule,
    TuiLoaderModule,
    TuiTooltipModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    HttpClientModule
  ],
  providers: [
    MessagesService
  ]
})
export class MessagesModule { }
