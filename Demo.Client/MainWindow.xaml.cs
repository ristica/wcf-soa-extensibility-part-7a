﻿using Demo.Contracts;
using Demo.Proxies;
using System.ComponentModel;
using System.Windows;
using System;

namespace Demo.Client
{
    public partial class MainWindow : Window, IProcessCallback
    {
        private ProcessDuplexClient _proxy;
        private bool _cancel;

        public MainWindow()
        {
            InitializeComponent();
            this._proxy = new ProcessDuplexClient(new System.ServiceModel.InstanceContext(this));
            this.BtnStart.IsEnabled = !(this._proxy.Connect());
        }

        private void StartProcess(object sender, RoutedEventArgs e)
        {            
            this._proxy.StartProcess();

            this._cancel = false;
            this.BtnStart.IsEnabled = false;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            this._proxy.Disconnect();

            this._cancel = true;
            this._proxy.Close();
        }

        private void CloseProcess(object sender, RoutedEventArgs e)
        {
            this._cancel = true;
        }

        public bool ReportBack(int nr)
        {
            if (nr > 0)
            {
                this.BtnStart.IsEnabled = false;
                this.lblOutput.Content = nr;
            }
            else
            {
                this.BtnStart.IsEnabled = true;
                this.lblOutput.Content = "Done";
                this._cancel = false;
            }            

            return this._cancel;
        }
    }
}
