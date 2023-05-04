import { Injectable } from '@angular/core';
import configUrl from '../../../assets/config/config.json';
import { HttpClient } from '@angular/common/http';
import {Webhook, WebhookRegistration} from "../types/webhook.interface";
import {Observable} from "rxjs";
import {AbstractControl, ValidationErrors, ɵElement, ɵFormGroupValue, ɵTypedOrUntyped} from "@angular/forms";

@Injectable({
  providedIn: 'root'
})
export class WebhookRegistrationService {
  config = {
    baseUrl : configUrl.apiUrl.baseUrl
  }
  constructor(private http: HttpClient) { }

  registerWebhook(registerWebhook: WebhookRegistration) : Observable<Webhook> {
    const url = this.config.baseUrl + '/webhooksubscriptions';
    return this.http.post<Webhook>(url, registerWebhook);
  }
}
