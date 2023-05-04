export interface Webhook{
  id?: number | null;
  webhookUri?: string | null;
  secret?: string | null;
  webhookType?: string | null;
  webhookPublisher?: string | null;
}

export interface WebhookRegistration {
  webhookUri?: string | null;
  webhookType?: string | null;
}
