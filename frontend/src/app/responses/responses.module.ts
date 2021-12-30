import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ResponsesRoutingModule } from './responses-routing.module';
import { ResponsesComponent } from './responses.component';
import { TuiInputModule, TuiIslandModule, TuiMultiSelectModule, TuiSelectModule, TuiTextAreaModule } from '@taiga-ui/kit';
import { ResponseComponent } from './response/response.component';
import { TuiButtonModule, TuiDataListModule, TuiDialogModule, TuiGroupModule, TuiLabelModule, TuiTextfieldControllerModule } from '@taiga-ui/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ResponsesService } from './services/responses.service';
import { SharedModule } from '../shared/shared.module';
import { HttpClientModule } from '@angular/common/http';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';


@NgModule({
  declarations: [
    ResponsesComponent,
    ResponseComponent
  ],
  imports: [
    CommonModule,
    ResponsesRoutingModule,
    TuiIslandModule,
    TuiInputModule,
    TuiButtonModule,
    TuiMultiSelectModule,
    TuiDataListModule,
    TuiDialogModule,
    TuiLabelModule,
    TuiTextfieldControllerModule,
    TuiGroupModule,
    TuiTextAreaModule,
    TuiSelectModule,
    FontAwesomeModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    HttpClientModule
  ],
  providers: [
    ResponsesService
  ]
})
export class ResponsesModule { }
