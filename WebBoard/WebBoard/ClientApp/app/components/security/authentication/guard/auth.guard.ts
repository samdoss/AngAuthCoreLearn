import { Injectable } from '@angular/core';
import { LocalStorageService } from 'angular-2-local-storage';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

import { AuthenticationService } from '../service/auth.service';

@Injectable()
export class AuthGuard implements CanActivate {

    constructor(
        private router: Router,
        private localStorageService: LocalStorageService,
        private AuthService: AuthenticationService
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | boolean {
        var tempRoute = state.url;

        if (this.localStorageService.get("accessToken")) {

            var user_name = this.localStorageService.get<string>("userEmail");
            var access_token = this.localStorageService.get<string>("accessToken");
            var refresh_token = this.localStorageService.get<string>("refreshToken");
            //Check Validate Token 
            if (this.AuthService.checkValidateToken(access_token)) {

                //Exist Token
                //Check Time Token
                var datetime = (Date.now() / 1000);
                var expires_time = this.localStorageService.get<number>("expiresTime");
                var remain_time = expires_time - datetime;

                if (remain_time > -1) {
                    //Refresh Token
                    if (remain_time < 1200) {

                        //api to refresh Token
                        return this.AuthService.refreshToken(user_name, access_token, refresh_token)
                            .map(
                            response => {
                                if (response) {
                                    this.localStorageService.set("accessToken", response.access_token);
                                    this.localStorageService.set("refreshToken", response.refresh_token);
                                    this.localStorageService.set("expiresTime", (Date.now() / 1000) + response.expires_in);
                                    console.log("refresh token");

                                    this.AuthService.checkRoleForRedirect(tempRoute);

                                    return true;
                                } else {
                                    //Refresh error
                                    //console.log("refresh token expire");
                                    if (tempRoute == "/login") {
                                        this.router.navigate(['/login']);
                                        return true;
                                    } else {
                                        this.router.navigate(['/login'], { queryParams: { tempUrl: tempRoute } });
                                        return false;
                                    }
                                }
                            }
                        )

                    } // exist token;
                    else {
                        this.AuthService.checkRoleForRedirect(tempRoute);
                        return true;
                    }
                } //expire token;
                else {

                    //Refresh token
                    //api to refresh Token
                    return this.AuthService.refreshToken(user_name, access_token, refresh_token).map(response => {
                        if (response) {
                            this.localStorageService.set("accessToken", response.access_token);
                            this.localStorageService.set("refreshToken", response.refresh_token);
                            this.localStorageService.set("expiresTime", (Date.now() / 1000) + response.expires_in);
                            //console.log("refresh token");

                            this.AuthService.checkRoleForRedirect(tempRoute);

                            return true;
                        } else {
                            //Refresh error
                            //console.log("refresh token expire");
                            if (tempRoute == "/login") {
                                this.router.navigate(['/login']);
                                return true;
                            } else {
                                this.router.navigate(['/login'], { queryParams: { tempUrl: tempRoute } });
                                return false;
                            }
                        }
                    })
                }

            } //Wrong Token
            else {

                if (tempRoute == "/login") {
                    this.router.navigate(['/login']);
                    return true;
                } else {
                    this.router.navigate(['/login'], { queryParams: { tempUrl: tempRoute } });
                    return false;
                }
            }
        } //not have token
        else {

            if (tempRoute == "/login") {
                this.router.navigate(['/login']);
                return true;
            } else {
                this.router.navigate(['/login'], { queryParams: { tempUrl: tempRoute } });
                return false;
            }

        }

    }

}