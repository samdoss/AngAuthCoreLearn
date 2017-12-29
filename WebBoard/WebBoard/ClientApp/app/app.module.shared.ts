import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Http, HttpModule, RequestOptions, XHRBackend } from '@angular/http';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { LocalStorageModule } from "angular-2-local-storage";
import { HttpService } from './components/security/loader/http.service';
import { LoaderService } from './components/security/loader/loader.service';

import { AlertModule } from 'ngx-bootstrap/alert';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { DatepickerModule } from 'ngx-bootstrap/datepicker';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { ProgressbarModule } from 'ngx-bootstrap/progressbar';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { TimepickerModule } from 'ngx-bootstrap/timepicker';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';

import { AppComponent } from './components/app/app.component';
import { AppRoutingModule } from './app-routing.module';

import { LoginComponent } from './components/security/login/login.component';

import { NavMenuComponent } from './components/navmenu/navmenu.component';
//import { HomeComponent } from './components/home/home.component';
//import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
//import { CounterComponent } from './components/counter/counter.component';
import { MainBoardComponent } from './components/mainboard/mainboard.component';
import { CreatePostComponent } from './components/createpost/createpost.component';
import { ViewPostComponent } from './components/viewpost/viewpost.component';

//Module
import { module_sample1Module } from './components/module_sample1/module_sample1.module';


// Provider
import { Configuration, Constants } from './app.constants';


//Authentication
import { AuthGuard } from './components/security/authentication/guard/auth.guard';
import { AuthenticationService } from './components/security/authentication/service/auth.service';


//Model
import { AuthData } from './components/security/authentication/model/auth.data'
import { TokenInterceptor } from "./components/security/authentication/service/interceptor.service";


@NgModule({
    declarations: [
        AppComponent,

        LoginComponent,

        NavMenuComponent,

        MainBoardComponent,
        CreatePostComponent,
        ViewPostComponent,
    ],
    imports: [
        HttpClientModule,
        ReactiveFormsModule,
        CommonModule,
        HttpModule,
        FormsModule,

        LocalStorageModule.withConfig({
            prefix: 'webboard',
            storageType: 'localStorage'
        }),
        
        AlertModule.forRoot(),
        ButtonsModule.forRoot(),
        CarouselModule.forRoot(),
        CollapseModule.forRoot(),
        DatepickerModule.forRoot(),
        BsDropdownModule.forRoot(),
        ModalModule.forRoot(),
        PaginationModule.forRoot(),
        PopoverModule.forRoot(),
        ProgressbarModule.forRoot(),
        TabsModule.forRoot(),
        TimepickerModule.forRoot(),
        TooltipModule.forRoot(),
        TypeaheadModule.forRoot(),

        module_sample1Module,

        AppRoutingModule,
    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },

        LoaderService,
        AuthenticationService,
        Configuration, Constants,
        AuthGuard,

    ]
})
export class AppModuleShared {
}
