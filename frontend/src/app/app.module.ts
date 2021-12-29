import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { TuiRootModule, TuiDialogModule, TuiNotificationsModule, TuiThemeNightModule, TuiModeModule, TuiButtonModule, TuiExpandModule } from "@taiga-ui/core";
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TuiAvatarModule, TuiIslandModule, TuiTabsModule, TuiToggleModule } from "@taiga-ui/kit";
import { FormsModule } from "@angular/forms";
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

@NgModule({
  declarations: [
    AppComponent
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
    TuiAvatarModule,
    FontAwesomeModule
],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
