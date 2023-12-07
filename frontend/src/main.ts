import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { provideAnimations } from '@angular/platform-browser/animations';

let config = appConfig;
config.providers = appConfig.providers.concat(provideAnimations());
bootstrapApplication(AppComponent, config).catch((err) => console.error(err));
