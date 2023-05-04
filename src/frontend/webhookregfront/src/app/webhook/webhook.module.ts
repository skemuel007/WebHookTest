import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WebookRegistrationPageComponent } from './component/webook-registration-page/webook-registration-page.component';
import {ReactiveFormsModule} from "@angular/forms";
import {Routes, RouterModule} from "@angular/router";
import {WebhookRegistrationService} from "../shared/services/webhook-registration.service";
import {ToastrModule} from "ngx-toastr";

const routes: Routes = [
  {
    path: '',
    component: WebookRegistrationPageComponent
  }
];

@NgModule({
  declarations: [
    WebookRegistrationPageComponent
  ],
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    ReactiveFormsModule,
    ToastrModule,
  ],
  providers: [
    WebhookRegistrationService
  ]
})
export class WebhookModule { }
