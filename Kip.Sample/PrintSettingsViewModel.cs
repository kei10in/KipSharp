using System;
using System.Linq;
using System.Printing;
using System.Reactive.Linq;
using ReactiveUI;

namespace Kip.Sample
{
    internal class PrintSettingsViewModel : ReactiveObject
    {
        private Ticket printTicket;

        public PrintSettingsViewModel()
        {
            var lps = new LocalPrintServer();
            var queue = lps.GetPrintQueue("Microsoft XPS Document Writer");
            var pc = Capabilities.Load(queue.GetPrintCapabilitiesAsXml());
            printTicket = Ticket.Load(queue.DefaultPrintTicket.GetXmlStream());

            var mediaSizeFeature = pc.Get(Psk.PageMediaSize);
            MediaSizeDisplayName = mediaSizeFeature.Get(Psk.DisplayName)?.AsString();
            MediaSizeCapabilities = new ReactiveList<MediaSizeViewModel>(
                from option in mediaSizeFeature.Options()
                select new MediaSizeViewModel(option));

            MediaSize = MediaSizeCapabilities.First(ms =>
            {
                return ms.Option.Name == printTicket.Get(Psk.PageMediaSize)?.First()?.Name;
            });

            this.WhenAnyValue(x => x.MediaSize)
                .Skip(1)
                .Subscribe(ms =>
                {
                    printTicket = printTicket.Set(Psk.PageMediaSize, ms.Option);
                });

            var copies = pc.Get(Psk.JobCopiesAllDocuments);
            CopiesDisplayName = copies.Get(Psk.DisplayName)?.AsString();
            CopiesMax = (copies.Get(Psf.MaxValue)?.AsInt()).GetValueOrDefault(1);
            CopiesMin = (copies.Get(Psf.MinValue)?.AsInt()).GetValueOrDefault(1);
            CopiesMultiple = (copies.Get(Psf.Multiple)?.AsInt()).GetValueOrDefault(1);
            Copies = (copies.Get(Psf.DefaultValue)?.AsInt()).GetValueOrDefault(1);

            this.WhenAnyValue(x => x.Copies)
                .Skip(1)
                .Subscribe(cp =>
                {
                    printTicket = printTicket.Set(Psk.JobCopiesAllDocuments, cp);
                });

            IncreaseCopies = ReactiveCommand.Create(
                this.WhenAny(_ => _.Copies, x => x.Value < CopiesMax));
            IncreaseCopies.Subscribe(_ =>
            {
                Copies += CopiesMultiple;
            });

            DecreaseCopies = ReactiveCommand.Create(
                this.WhenAny(_ => _.Copies, x => x.Value > CopiesMin));
            DecreaseCopies.Subscribe(_AppDomain =>
            {
                Copies -= CopiesMultiple;
            });
        }

        public string MediaSizeDisplayName
        {
            get;
        }

        public ReactiveList<MediaSizeViewModel> MediaSizeCapabilities
        {
            get;
        }

        private MediaSizeViewModel _mediaSize;

        public MediaSizeViewModel MediaSize
        {
            get { return _mediaSize; }
            set { this.RaiseAndSetIfChanged(ref _mediaSize, value); }
        }

        public string CopiesDisplayName { get; }

        public int CopiesMax { get; }

        public int CopiesMin { get; }

        public int CopiesMultiple { get; }

        private int _copies;

        public int Copies
        {
            get { return _copies; }
            set { this.RaiseAndSetIfChanged(ref _copies, value); }
        }

        public ReactiveCommand<object> IncreaseCopies { get; }

        public ReactiveCommand<object> DecreaseCopies { get; }
    }
}
