import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { Categoria } from 'src/model/categoria';
import { Usuario } from 'src/model/usuario';

const baseUrl = "https://localhost:5001/api";

const apiUrl = `${baseUrl}/categorias`;
const apiLoginUrl = `${baseUrl}/autoriza/login`;

let httpOptions = {headers: new HttpHeaders({"Content-Type": "application/json"})};

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  montaHeaderToken() {
    const token = localStorage.getItem("jwt");
    httpOptions = {headers: new HttpHeaders({"Authorization": "Bearer " + token,"Content-Type": "application/json"})};
  }

  login (usuario: Usuario): Observable<Usuario> {
    return this.http.post<Usuario>(apiLoginUrl, usuario).pipe(
      catchError(this.handleError<Usuario>('Login'))
    );
  }

  getCategorias (): Observable<Categoria[]> {
    this.montaHeaderToken();
    return this.http.get<Categoria[]>(apiUrl, httpOptions)
      .pipe(catchError(this.handleError('getCategorias', [])));
  }

  getCategoria(id: number): Observable<Categoria> {
    this.montaHeaderToken();
    const url = `${apiUrl}/${id}`;
    return this.http.get<Categoria>(url, httpOptions).pipe(
      catchError(this.handleError<Categoria>(`getCategoria id=${id}`))
    );
  }

  addCategoria (categoria: Categoria): Observable<Categoria> {
    this.montaHeaderToken();
    return this.http.post<Categoria>(apiUrl, categoria, httpOptions).pipe(
      tap((categoria: Categoria) => console.log(`adicionou a Categoria com w/ id=${categoria.categoriaId}`)),
      catchError(this.handleError<Categoria>('addCategoria'))
    );
  }

  updateCategoria(id: number, categoria: Categoria): Observable<any> {
    this.montaHeaderToken();
    const url = `${apiUrl}/${id}`;
    return this.http.put(url, categoria, httpOptions).pipe(
      catchError(this.handleError<Categoria>('updateCategoria'))
    );
  }

  deleteCategoria (id: number): Observable<Categoria> {
    const url = `${apiUrl}/${id}`;
    return this.http.delete<Categoria>(url, httpOptions).pipe(
      tap(_ => console.log(`remove o Categoria com id=${id}`)),
      catchError(this.handleError<Categoria>('deleteCategoria'))
    );
  }

  private handleError<T> (operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      return of(result as T);
    };
  }
}