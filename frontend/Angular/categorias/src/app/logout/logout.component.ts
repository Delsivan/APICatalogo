import { ApiService } from 'src/service/api.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, NgForm } from '@angular/forms';

@Component({
  selector: 'logout',
  templateUrl: './logout.component.html'
})
export class LogoutComponent{
  logoutForm: FormGroup;
  isLoadingResults = false;
  constructor(private router: Router, private api: ApiService, 
    private formBuilder: FormBuilder) {
      this.logoutForm = this.formBuilder.group({});
    }

  addLogout() {
    localStorage.removeItem("jwt");
    this.isLoadingResults = true;
    this.router.navigate(['/login']);
  }
}