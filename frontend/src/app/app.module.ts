import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { TuiRootModule, TuiDialogModule, TuiNotificationsModule, TuiThemeNightModule, TuiModeModule, TuiButtonModule, TuiExpandModule } from "@taiga-ui/core";
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TuiAvatarModule, TuiIslandModule, TuiTabsModule, TuiToggleModule } from "@taiga-ui/kit";
import { FormsModule } from "@angular/forms";
import { ResponsesComponent } from './responses/responses.component';
import { ReactsComponent } from './reacts/reacts.component';
import { MessagesComponent } from './messages/messages.component';

@NgModule({
  declarations: [
    AppComponent,
    ResponsesComponent,
    ReactsComponent,
    MessagesComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    TuiRootModule,
    TuiThemeNightModule,
    TuiModeModule,
    BrowserAnimationsModule,
    TuiDialogModule,
    TuiNotificationsModule,
    TuiToggleModule,
    FormsModule,
    TuiTabsModule,
    TuiButtonModule,
    TuiIslandModule,
    TuiExpandModule,
    TuiAvatarModule
],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
