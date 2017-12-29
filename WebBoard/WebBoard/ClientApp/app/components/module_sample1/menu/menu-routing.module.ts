import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MenuComponent } from './menu.component';
//import { HttpClientModule, HttpClient } from '@angular/common/http';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from "../../security/authentication/service/interceptor.service";
//import { TokenInterceptor } from '../security/authentication/service/interceptor.service';

const detailsRoutes: Routes = [
    {
        path: '', component: MenuComponent,
        children: [
            {
                path: '',
                redirectTo: '/module_sample1/menu',
                pathMatch: 'full'
            },
            {
                path: 'menu',
                component: MenuComponent
            },
        ]
    },
    { path: "**", redirectTo: '/login' }
];
@NgModule({
    imports: [
        RouterModule.forChild(detailsRoutes)
    ],
    exports: [
        RouterModule
    ],
    providers: [
    ]
})
export class MenuRoutingModule { }
