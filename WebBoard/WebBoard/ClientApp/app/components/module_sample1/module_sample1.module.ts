import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Http, HttpModule } from '@angular/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { module_sample1Component } from './module_sample1.component';
import { module_sample1RoutingModule } from './module_sample1-routing.module';

import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { TokenInterceptor } from "../security/authentication/service/interceptor.service";
 
const APP_PROVIDERS: any = [

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

        module_sample1RoutingModule,
    ],
    declarations: [
        module_sample1Component,

    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
        //APP_PROVIDERS,

    ]
})

export class module_sample1Module { }
