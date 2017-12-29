import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Http, HttpModule } from '@angular/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MenuComponent } from './menu.component';
import { MenuRoutingModule } from './menu-routing.module';

import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { TokenInterceptor } from "../../security/authentication/service/interceptor.service";
import { AuthenticationService } from "../../security/authentication/service/auth.service";
import { AuthGuard } from "../../security/authentication/guard/auth.guard";
import { LoaderService } from "../../security/loader/loader.service";
import { Configuration } from "../../../app.constants";
import { HttpService } from "../../security/loader/http.service";

const APP_PROVIDERS : any = [

];
import { HttpClientModule, HttpClient } from '@angular/common/http';

import { CommonModule } from '@angular/common';
@NgModule({
    imports: [
        CommonModule,
        HttpModule, 
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        MenuRoutingModule
    ],
    declarations: [
        MenuComponent
    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },

        ////HttpService,
        //LoaderService,

        //Configuration,

        //AuthGuard,
        //AuthenticationService,
        ////APP_PROVIDERS
    ]
})

export class MenuModule { }
