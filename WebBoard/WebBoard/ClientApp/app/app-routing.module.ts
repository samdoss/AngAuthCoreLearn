import { NgModule, ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

//<<< ------- IMPORT Route PAGE ------- >>>
import { LoginComponent } from '../app/components/security/login/login.component';
import { LocalStorageService } from 'angular-2-local-storage';
// Using "data" in Routes to pass in <title> <meta> <link> tag information
// Note: This is only happening for ROOT level Routes, Add some additional logic if wanted this for Child level routing
// When change Routes it will automatically append these to document on the Server-side
const routes: Routes = [
    {
        path: '',
        redirectTo: '/login',
        pathMatch: 'full'
    },
    {
        path: 'login',
        component: LoginComponent,
        data: {
            title: 'webboard login',
            meta: [{ name: 'description', content: 'webboard login' }],
            links: [
                { rel: 'canonical', href: 'http://webboard.on.lk/' },
                { rel: 'alternate', hreflang: 'es', href: 'http://webboard.on.lk/' }
            ]
        }
    },
    { path: '**', component: LoginComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})

export class AppRoutingModule { }
