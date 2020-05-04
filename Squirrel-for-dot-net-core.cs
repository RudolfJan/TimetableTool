        async Task checkForSquirrelUpdates()
        {
            try {
                var (exitCode, output) = await RunSquirrel("--update https://example.org/updates");

                if (exitCode != 0) {
                    // TODO: Log update errors to Sentry
                    this.Log().Warn($"Failed to update! {output}");
                }
            } catch (Exception ex) {
                this.Log().Error(ex, $"Failed to invoke Update.exe");
            }
        }

        public Task<(int exitCode, string output)> RunSquirrel(string args)
        {
            string updateDotExe;
            try {
                updateDotExe = Path.GetFullPath("../Update.exe", Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

                if (!File.Exists(updateDotExe)) throw new Exception("update.exe doesn't exist");
            } catch (Exception ex) {
                this.Log().Warn(ex, $"Update.exe not found, this is probably not a deployed app");
                return Task.FromException<(int, string)>(new Exception("Not a deployed app"));
            }

            return invokeProcessAsync(updateDotExe, args, CancellationToken.None, Path.GetDirectoryName(updateDotExe));
        }

        public bool IsAppDeployed()
        {
            var updateDotExe = Path.GetFullPath("../Update.exe", Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
            return File.Exists(updateDotExe);
        }

        static Task<(int exitCode, string output)> invokeProcessAsync(string fileName, string arguments, CancellationToken ct, string workingDirectory = "")
        {
            var psi = new ProcessStartInfo(fileName, arguments);

            psi.UseShellExecute = false;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.ErrorDialog = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.WorkingDirectory = workingDirectory;

            return invokeProcessAsync(psi, ct);
        }

        static async Task<(int exitCode, string output)> invokeProcessAsync(ProcessStartInfo psi, CancellationToken ct)
        {
            var pi = Process.Start(psi);
            await Task.Run(() => {
                while (!ct.IsCancellationRequested) {
                    if (pi.WaitForExit(2000)) return;
                }

                if (ct.IsCancellationRequested) {
                    pi.Kill();
                    ct.ThrowIfCancellationRequested();
                }
            });

            string textResult = await pi.StandardOutput.ReadToEndAsync();
            if (String.IsNullOrWhiteSpace(textResult) || pi.ExitCode != 0) {
                textResult = (textResult ?? "") + "\n" + await pi.StandardError.ReadToEndAsync();

                if (String.IsNullOrWhiteSpace(textResult)) {
                    textResult = String.Empty;
                }
            }

            return (pi.ExitCode, textResult.Trim());
        }
    }
