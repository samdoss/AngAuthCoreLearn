import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Http, HttpModule } from '@angular/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { test_component_1Component } from './test_component_1.component';
import { test_component_1RoutingModule } from './test_component_1-routing.module';

const APP_PROVIDERS : any = [

];

@NgModule({
    imports: [
        test_component_1RoutingModule
    ],
    declarations: [
        test_component_1Component
    ],
    providers: [
        APP_PROVIDERS
    ]
})

export class test_component_1Module { }
