import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FilterableContainerComponent } from './components/filterable-container/filterable-container.component';
import { TuiIslandModule } from '@taiga-ui/kit';
import { TuiButtonModule, TuiLoaderModule } from '@taiga-ui/core';
import { ConfirmationDialogComponent } from './components/confirmation-dialog/confirmation-dialog.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { JsonParser } from './json/json-parser';
import { JsonHttpInterceptor } from './json/json.interceptor';



@NgModule({
  declarations: [
    FilterableContainerComponent,
    ConfirmationDialogComponent
  ],
  imports: [
    CommonModule,
    TuiIslandModule,
    TuiLoaderModule,
    TuiButtonModule
  ],
  exports: [
    FilterableContainerComponent
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JsonHttpInterceptor, multi: true },
    { provide: JsonParser, useClass: JsonParser }
  ]
})
export class SharedModule { }
