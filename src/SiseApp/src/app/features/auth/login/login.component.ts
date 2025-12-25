import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import {MatSnackBar} from '@angular/material/snack-bar';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'], // Asegúrate de enlazar el SCSS
})
export class LoginComponent {
  form: FormGroup;
  loading = false;
  returnUrl = '/';
  hidePassword = true; // Nueva propiedad para el toggle de contraseña

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private snack: MatSnackBar
  ) {
    this.form = this.fb.group({
      userNameOrEmail: ['', [Validators.required]], // Sugerencia: validar email si aplica
      password: ['', [Validators.required]],
      rememberMe: [false] // Agregado para el checkbox
    });

    const q = this.route.snapshot.queryParamMap.get('returnUrl');
    if (q) this.returnUrl = q;
  }

  submit() {
    if (this.form.invalid) return;
    this.loading = true;
    const { userNameOrEmail, password } = this.form.value;
    
    this.auth.login(userNameOrEmail, password).subscribe({
      next: () => {
        this.loading = false;
        this.snack.open('Bienvenido', 'Cerrar', { duration: 2000 });
        this.router.navigateByUrl(this.returnUrl);
      },
      error: (err) => {
        this.loading = false;
        const msg = err?.error?.message ?? 'Credenciales incorrectas';
        this.snack.open(msg, 'Cerrar', { duration: 3000 });
      }
    });
  }
}