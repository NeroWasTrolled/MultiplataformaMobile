using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MultiplataformaMobile.Models;
using MultiplataformaMobile.Services;
using MultiplataformaMobile.Views;

namespace MultiplataformaMobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageCadastrar : ContentPage
    {
        public PageCadastrar()
        {
            InitializeComponent();
        }

        public PageCadastrar(ModelMatricula nota)
        {
            InitializeComponent();
            btSalvar.Text = "Alterar";
            txtCodigo.IsVisible = true;
            btExcluir.IsVisible = true;
            txtCodigo.Text = nota.id.ToString();
            txtMatricula.Text = nota.matricula;
        }


        private void btSalvar_Clicked(object sender, EventArgs e)
        {
            try
            {
                ModelMatricula notas = new ModelMatricula();
                notas.matricula = txtMatricula.Text;
                notas.epi = txtEpi.Text;
                ServiceDBMatricula dBNotas = new ServiceDBMatricula(App.DbPath);
                if (btSalvar.Text == "Inserir")
                {
                    dBNotas.Inserir(notas);
                    DisplayAlert("Resultado", dBNotas.StatusMessage, "OK");
                }
                else
                {
                    notas.id = Convert.ToInt32(txtCodigo.Text);
                    dBNotas.Alterar(notas);
                    DisplayAlert("Funcionario alterado com sucesso!", dBNotas.StatusMessage, "OK");
                }
                MasterDetailPage p = (MasterDetailPage)Application.Current.MainPage;
                p.Detail = new NavigationPage(new PageHome());
            }
            catch (Exception ex)
            {
                DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async void btExcluir_Clicked(object sender, EventArgs e)
        {
            var resp = await DisplayAlert("Excluir Funcionario", "Deseja EXCLUIR este funcionario selecionado?", "Sim", "Não");
            if (resp == true)
            {
                ServiceDBMatricula dBNotas = new ServiceDBMatricula(App.DbPath);
                int id = Convert.ToInt32(txtCodigo.Text);
                dBNotas.Excluir(id);
                DisplayAlert("Funcionario excluído com sucesso", dBNotas.StatusMessage, "OK");
                MasterDetailPage p = (MasterDetailPage)Application.Current.MainPage;
                p.Detail = new NavigationPage(new PageHome());
            }
        }

        private void btCancelar_Clicked(object sender, EventArgs e)
        {
            MasterDetailPage p = (MasterDetailPage)Application.Current.MainPage;
            p.Detail = new NavigationPage(new PageHome());
        }

        private void DatePickerEntrega_DateSelected(object sender, DateChangedEventArgs e)
        {
            AtualizarContagemRegressiva();
        }

        private void DatePickerVencimento_DateSelected(object sender, DateChangedEventArgs e)
        {
            AtualizarContagemRegressiva();
        }

        private void AtualizarContagemRegressiva()
        {
            DateTime dataEntrega = datePickerEntrega.Date;
            DateTime dataVencimento = datePickerVencimento.Date;

            TimeSpan diff = dataVencimento - dataEntrega;
            int diasRestantes = (int)Math.Ceiling(diff.TotalDays);

            if (diasRestantes >= 0)
            {
                labelCountdown.Text = $"{diasRestantes} dias restantes para o vencimento.";
            }
            else
            {
                labelCountdown.Text = "Data de vencimento anterior à data de entrega.";
            }
        }
    }
}