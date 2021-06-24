import { Categoria } from 'src/model/categoria';
import { Component } from '@angular/core';
import { FormGroup, Validators, NgForm, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiService } from 'src/service/api.service';

@Component({
  selector: 'app-categoria-nova',
  templateUrl: './categoria-nova.component.html',
  styleUrls: ['./categoria-nova.component.scss']
})
export class CategoriaNovaComponent {
  categoriaForm: FormGroup;
  nome: String = '';
  imagemUrl: String = '';
  isLoadingResults = false;

  constructor(private router: Router, private api: ApiService, private formBuilder: FormBuilder) {
    this.categoriaForm = this.formBuilder.group({
      'nome' : [null, Validators.required],
      'imagemUrl' : [null, Validators.required]
    });
   }

  addCategoria(form: Categoria) {
    this.isLoadingResults = true;
    this.api.addCategoria(form)
      .subscribe(res => {
          this.isLoadingResults = false;
          this.router.navigate(['/categorias']);
        }, (err) => {
          console.log(err);
          this.isLoadingResults = false;
        });
  }
}
