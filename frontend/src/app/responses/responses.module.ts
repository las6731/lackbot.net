import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ResponsesRoutingModule } from './responses-routing.module';
import { ResponsesComponent } from './components/responses/responses.component';
import { TuiInputModule, TuiIslandModule, TuiMultiSelectModule, TuiSelectModule, TuiTextAreaModule } from '@taiga-ui/kit';
import { ResponseComponent } from './components/response/response.component';
import { TuiButtonModule, TuiDataListModule, TuiDialogModule, TuiGroupModule, TuiLabelModule, TuiLoaderModule, TuiTextfieldControllerModule, TuiTooltipModule } from '@taiga-ui/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ResponsesService } from './services/responses.service';
import { SharedModule } from '../shared/shared.module';
import { HttpClientModule } from '@angular/common/http';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { DialogComponent } from './components/dialog/dialog.component';


@NgModule({
  declarations: [
    ResponsesComponent,
    ResponseComponent,
    DialogComponent
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
    TuiLoaderModule,
    TuiTooltipModule,
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
