import { Component, OnInit, ViewChild, Output, EventEmitter } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap';

/* Import service */
import { AuthenticationService } from '../authentication/service/auth.service';

/* Import model */
import { RegisterData } from './model/v1/register.data';
import { AuthenModel } from '../authen-modal/model/v1/authen-model';

@Component({
    selector: 'register-modal',
    templateUrl: './register.html',
    styleUrls: ['./register.scss']

})

export class RegisterComponent {

    constructor(
        private AuthService: AuthenticationService
    ) { }
    AuthenModel: AuthenModel = new AuthenModel();
    @ViewChild('RegisterModal') public RegisterModal: ModalDirective;
    @Output('CloseRegisterModal') sentCheck = new EventEmitter<AuthenModel>();
    UserData: RegisterData = new RegisterData();

    //Validate
    isUsername: boolean = true;
    isSurname: boolean = true;
    isEmail: boolean = true;
    isPassword: boolean = true;
    isLengthPassword: boolean = true;
    isRePassword: boolean = true;
    isMatchPassword: boolean = true;
    isEnable: boolean = true;

    register() {
        //Check Validation
        this.ValidateData();
        if (this.isUsername && this.isSurname && this.isEmail && this.isPassword
            && this.isLengthPassword && this.isRePassword && this.isMatchPassword) {

            //Register User
            //console.log("Register");
            this.AuthService.registerUser(this.UserData).subscribe(
                response => {
                    //console.log(response);
                    this.RegisterModal.hide();
                    this.AuthenModel.user_email = this.UserData.Email;
                    this.AuthenModel.isModel = "Regis";
                    this.sentCheck.emit(this.AuthenModel);
                }, error => {
                    //response => {
                    console.log("Error");
                    //}
                }
            );
            //Clear Data
            this.UserData = new RegisterData();
            return true;
        }
    }

    show() {
        this.RegisterModal.show();
    }

    ValidateData() {
        this.ValidateFirstname();
        this.ValidateLastname();

        this.ValidateEmail();
        this.ValidatePassword();
        this.ValidateRePassword();
    }

    ValidateFirstname() {
        //Check Validate
        if (this.UserData.Firstname != "") {
            this.isUsername = true;
        } else {
            this.isUsername = false;
        }
    }

    ValidateLastname() {
        //Check Validate
        if (this.UserData.Lastname != "") {
            this.isSurname = true;
        } else {
            this.isSurname = false;
        }
    }

    ValidateEmail() {
        //Check Validate
        if (this.UserData.Email != "") {
            this.isEmail = true;
        } else {
            this.isEmail = false;
        }
    }

    ValidatePassword() {
        //Check Validate
        if (this.UserData.Password != "") {
            this.isPassword = true;
            //Check Length
            if (this.UserData.Password.length >= 6) {
                this.isLengthPassword = true;
            } else {
                this.isLengthPassword = false;
            }
            //Check Match
            if (this.UserData.RePassword != "") {
                if (this.UserData.Password == this.UserData.RePassword) {
                    this.isMatchPassword = true;
                } else {
                    this.isMatchPassword = false;
                }
            }

        } else {
            this.isPassword = false;
            this.isLengthPassword = true;
        }
    }

    ValidateRePassword() {
        //Check Validate
        if (this.UserData.RePassword != "") {
            this.isRePassword = true;
            if (this.UserData.Password == this.UserData.RePassword) {
                this.isMatchPassword = true;
            } else {
                this.isMatchPassword = false;
            }
        } else {
            this.isRePassword = false;
            this.isMatchPassword = true;
        }
    }
}