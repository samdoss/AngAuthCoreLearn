import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Http, HttpModule } from '@angular/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { test_component_2Component } from './test_component_2.component';
import { test_component_2RoutingModule } from './test_component_2-routing.module';
import { sub_test_component_2_1Component } from "./sub_test_component_2_1/sub_test_component_2_1.component";

const APP_PROVIDERS : any = [

];

@NgModule({
    imports: [
        test_component_2RoutingModule
    ],
    declarations: [
        test_component_2Component,
        sub_test_component_2_1Component
    ],
    providers: [
        APP_PROVIDERS
    ]
})

export class test_component_2Module { }
