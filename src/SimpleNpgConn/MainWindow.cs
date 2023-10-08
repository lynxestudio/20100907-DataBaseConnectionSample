using System;
using Gtk;
using Npgsql;

public partial class MainWindow : Gtk.Window
{	
	public MainWindow () : base(Gtk.WindowType.Toplevel)
	{
		Build ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
	protected virtual void OnBtnCancelClicked (object sender, System.EventArgs e)
	{
		Application.Quit();
	}
	
	protected virtual void OnBtnOkClicked (object sender, System.EventArgs e)
	{
	try{
	NpgsqlConnectionStringBuilder connString = new NpgsqlConnectionStringBuilder();
	connString.Host = txtServer.Text;
	connString.Database = @txtDataBase.Text;
	connString.UserName = @txtUserID.Text;
	connString.Password = @txtPassWord.Text;
	connString.Timeout = (string.IsNullOrEmpty(txtTimeOut.Text) ? 0 : Convert.ToInt32(txtTimeOut.Text));
	connString.Pooling = this.chkPooling.Active;
	connString.MinPoolSize = (String.IsNullOrEmpty(txtMinPool.Text) ? 0 : Convert.ToInt32(txtMinPool.Text));
	connString.MaxPoolSize = (String.IsNullOrEmpty(txtMaxPool.Text) ? 0 : Convert.ToInt32(txtMaxPool.Text));
	TimeSpan tsinicon;
		using(NpgsqlConnection conn = new NpgsqlConnection(connString.ToString())){
		conn.Open();
		tsinicon = System.DateTime.Now.TimeOfDay;
		if(conn.State ==  System.Data.ConnectionState.Open)
					MessageBox("Conexión exitosa",MessageType.Info);
		}
		TimeSpan tsfin = System.DateTime.Now.TimeOfDay.Subtract(tsinicon);
		lbStatus.Text = string.Format("Status conectado {0} milisegs.",tsfin.Milliseconds.ToString());
	}
	catch(NpgsqlException ex){ 
			MessageBox("Base de datos " + ex.Message ,MessageType.Error); 
			lbStatus.Text = "No conectado.";
	}
	catch(ApplicationException ex){ 
			MessageBox("Aplicación " + ex.Message,MessageType.Error);
			lbStatus.Text = "No conectado.";
	}
	catch(Exception ex){
			MessageBox("General " + ex.Message,MessageType.Error);
			lbStatus.Text = "No conectado.";
		}
	}
	
	void MessageBox(string msg,MessageType msgtype){
		 using(Dialog messageBox = new MessageDialog(this,
			         DialogFlags.DestroyWithParent,
			         msgtype,
			         ButtonsType.Ok,
			         msg)){
				messageBox.Run();
				messageBox.Destroy();
			}
	}
	
	protected virtual void OnChkPoolingToggled (object sender, System.EventArgs e)
	{
		if(chkPooling.Active){
		txtMinPool.Show();
		txtMaxPool.Show();
		}
		else{
		txtMinPool.Hide();
		txtMaxPool.Hide();
		}
	}
	
	
}

