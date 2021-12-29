import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ResponsesRoutingModule } from './responses-routing.module';
import { ResponsesComponent } from './responses.component';
import { TuiInputModule, TuiIslandModule, TuiMultiSelectModule } from '@taiga-ui/kit';
import { ResponseComponent } from './response/response.component';
import { TuiButtonModule, TuiDataListModule } from '@taiga-ui/core';
import { FormsModule } from '@angular/forms';
import { ResponsesService } from './services/responses.service';
import { SharedModule } from '../shared/shared.module';
import { HttpClientModule } from '@angular/common/http';


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
    FormsModule,
    SharedModule,
    HttpClientModule
  ],
  providers: [
    ResponsesService
  ]
})
export class ResponsesModule { }
