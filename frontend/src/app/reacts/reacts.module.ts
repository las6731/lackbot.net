import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReactsRoutingModule } from './reacts-routing.module';
import { ReactsComponent } from './components/reacts/reacts.component';
import { ReactComponent } from './components/react/react.component';
import { ReactDialogComponent } from './components/react-dialog/react-dialog.component';
import { TuiInputModule, TuiIslandModule, TuiMultiSelectModule, TuiSelectModule } from '@taiga-ui/kit';
import { TuiButtonModule, TuiDataListModule, TuiLabelModule, TuiLoaderModule, TuiTextfieldControllerModule, TuiTooltipModule } from '@taiga-ui/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { HttpClientModule } from '@angular/common/http';
import { ReactsService } from './services/reacts.service';


@NgModule({
  declarations: [
    ReactsComponent,
    ReactComponent,
    ReactDialogComponent
  ],
  imports: [
    CommonModule,
    ReactsRoutingModule,
    TuiIslandModule,
    TuiInputModule,
    TuiButtonModule,
    TuiMultiSelectModule,
    TuiDataListModule,
    TuiLabelModule,
    TuiTextfieldControllerModule,
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
    ReactsService
  ]
})
export class ReactsModule { }
