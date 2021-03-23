import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ReactsComponent } from './reacts/reacts.component';
import { ResponsesComponent } from './responses/responses.component';

const routes: Routes = [
    { path: '', redirectTo: 'responses', pathMatch: 'full' },
    { path: 'responses', component: ResponsesComponent },
    { path: 'reacts', component: ReactsComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
