import {Component, OnInit} from '@angular/core';
import {WebhookRegistrationService} from "../../../shared/services/webhook-registration.service";
import {FormBuilder, Validators} from "@angular/forms";
import {Title} from "@angular/platform-browser";
import {Router} from "@angular/router";
import {ToastrService} from "ngx-toastr";
import {HttpErrorResponse} from "@angular/common/http";
import {Webhook} from "../../../shared/types/webhook.interface";

@Component({
  selector: 'app-webook-registration-page',
  templateUrl: './webook-registration-page.component.html',
  styleUrls: ['./webook-registration-page.component.css']
})
export class WebookRegistrationPageComponent implements  OnInit{

  webhookData: Webhook | undefined;
  webhookFormGroup = this.fb.group({
    webhookUri: ['', Validators.required],
    webhookType: ['', Validators.required]
  });

  loading = false;
  priceChange: string | null = "priceChange";
  seatAvailability: string | null = "seatAvailability";
  flightDelayDisruption: string | null = "flightDelayDisruption";

  constructor(
    private fb: FormBuilder,
    public webhookService: WebhookRegistrationService,
    private titleService: Title,
    private toastrService: ToastrService) {}

  ngOnInit() {
    this.titleService.setTitle('Webhook Registration');
    this.webhookData = undefined;
    console.log(this.webhookData);
  }
  submit(): void {
    this.loading = true;
    this.webhookService.registerWebhook(this.webhookFormGroup.value)
      .subscribe({
        next: (webhook) => {
          this.toastrService.success("Success webhook registered", "Webhook Registration", { timeOut: 9000});
          this.loading = false;
          this.webhookFormGroup.reset();
          this.webhookData = webhook;
        },
        error: (err: HttpErrorResponse) => {
          this.loading = false;
          this.webhookData = undefined;
          console.log(err);
          if (err.status !== 0) {
            this.toastrService.error(err.error.detail, "Webhook Registration Error", {timeOut: 9000});
          }
        }
      })
  }
}
