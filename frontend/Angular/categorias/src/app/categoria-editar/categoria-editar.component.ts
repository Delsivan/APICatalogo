import { Categoria } from 'src/model/categoria';
import { ApiService } from 'src/service/api.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, NgForm, FormBuilder } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-categoria-editar',
  templateUrl: './categoria-editar.component.html',
  styleUrls: ['./categoria-editar.component.scss']
})
export class CategoriaEditarComponent implements OnInit {
  categoriaId: number = 0;
  categoriaForm: FormGroup;
  nome: String = '';
  imagemUrl: String = '';
  isLoadingResults = false;

  constructor(private router: Router, private route: ActivatedRoute,
     private api: ApiService, private formBuilder: FormBuilder) {
      this.categoriaForm = this.formBuilder.group({
        'categoriaId' : [null],  
        'nome' : [null, Validators.required],
        'imagemUrl' : [null, Validators.required]
      });
    }

  ngOnInit() {
    this.getCategoria(this.route.snapshot.params['id']);
 }

 getCategoria(id:number) {
  this.api.getCategoria(id).subscribe(data => {
    this.categoriaId = data.categoriaId;
    this.categoriaForm.setValue({
      categoriaId: data.categoriaId,
      nome: data.nome,
      imagemUrl : data.imagemUrl,
    });
  });
}

updateCategoria(form: Categoria) {
  this.isLoadingResults = true;
  this.api.updateCategoria(this.categoriaId, form)
    .subscribe(res => {
        this.isLoadingResults = false;
        this.router.navigate(['/categoria-detalhe/' + this.categoriaId]);
      }, (err) => {
        console.log(err);
        this.isLoadingResults = false;
      }
    );
 }
}