import { ApiService } from 'src/service/api.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, NgForm, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Usuario } from 'src/model/usuario';

@Component({
  selector: 'login',
  templateUrl: './login.component.html'
})
export class LoginComponent {
  loginForm: FormGroup;
  email: String = '';
  password: String = '';
  dataSource: Usuario;
  isLoadingResults = false;

  constructor(private router: Router, private api: ApiService,
     private formBuilder: FormBuilder) {
      this.loginForm = this.formBuilder.group({
        'email' : [null, Validators.required],
        'password' : [null, Validators.required]
      });
      this.dataSource = new Usuario();
    }

  addLogin(form: Usuario) {
    console.log(form)
    this.isLoadingResults = true;
    this.api.login(form)
      .subscribe(res => {
          this.dataSource = res;
          localStorage.setItem("jwt", res.token);
          this.isLoadingResults = false;
          this.router.navigate(['/categorias']);
        }, (err) => {
          console.log(err);
          this.isLoadingResults = false;
        });
  }
}