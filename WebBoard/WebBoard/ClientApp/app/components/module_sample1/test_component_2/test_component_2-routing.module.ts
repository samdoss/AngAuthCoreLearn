import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { test_component_2Component } from './test_component_2.component';
import { sub_test_component_2_1Component } from "./sub_test_component_2_1/sub_test_component_2_1.component";

const detailsRoutes: Routes = [
    {
        path: '', component: test_component_2Component,
        children: [
            {
                path: '',
                redirectTo: '/module_sample1/test_component_2',
                pathMatch: 'full'
            },
            {
                path: 'test_component_2',
                component: test_component_2Component
            },
            {
                path: 'sub_test_component_2_1Component',
                component: sub_test_component_2_1Component
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
    ]
})
export class test_component_2RoutingModule { }
