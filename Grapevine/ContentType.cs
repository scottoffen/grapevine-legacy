using System;
using System.Reflection;

namespace Grapevine
{
    public enum ContentType
    {
        [Metadata(Value = "application/x-authorware-bin", IsBinary = true)]
        AAB,

        [Metadata(Value = "audio/x-aac", IsBinary = true)]
        AAC,

        [Metadata(Value = "application/x-authorware-map", IsBinary = true)]
        AAM,

        [Metadata(Value = "application/x-authorware-seg", IsBinary = true)]
        AAS,

        [Metadata(Value = "application/x-abiword", IsBinary = true)]
        ABW,

        [Metadata(Value = "application/pkix-attr-cert", IsBinary = true)]
        AC,

        [Metadata(Value = "application/vnd.americandynamics.acc", IsBinary = true)]
        ACC,

        [Metadata(Value = "application/x-ace-compressed", IsBinary = true)]
        ACE,

        [Metadata(Value = "application/vnd.acucobol", IsBinary = true)]
        ACU,

        [Metadata(Value = "application/vnd.acucorp", IsBinary = true)]
        ACUTC,

        [Metadata(Value = "audio/adpcm", IsBinary = true)]
        ADP,

        [Metadata(Value = "application/vnd.audiograph", IsBinary = true)]
        AEP,

        [Metadata(Value = "application/x-font-type1", IsBinary = true)]
        AFM,

        [Metadata(Value = "application/vnd.ibm.modcap", IsBinary = true)]
        AFP,

        [Metadata(Value = "application/vnd.ahead.space", IsBinary = true)]
        AHEAD,

        [Metadata(Value = "application/postscript", IsBinary = true)]
        AI,

        [Metadata(Value = "audio/x-aiff", IsBinary = true)]
        AIF,

        [Metadata(Value = "audio/x-aiff", IsBinary = true)]
        AIFC,

        [Metadata(Value = "audio/x-aiff", IsBinary = true)]
        AIFF,

        [Metadata(Value = "application/vnd.adobe.air-application-installer-package+zip", IsBinary = true)]
        AIR,

        [Metadata(Value = "application/vnd.dvb.ait", IsBinary = true)]
        AIT,

        [Metadata(Value = "application/vnd.amiga.ami", IsBinary = true)]
        AMI,

        [Metadata(Value = "application/vnd.android.package-archive", IsBinary = true)]
        APK,

        [Metadata(Value = "text/cache-manifest", IsText = true)]
        APPCACHE,

        [Metadata(Value = "application/x-ms-application", IsBinary = true)]
        APPLICATION,

        [Metadata(Value = "application/vnd.lotus-approach", IsBinary = true)]
        APR,

        [Metadata(Value = "application/x-freearc", IsBinary = true)]
        ARC,

        [Metadata(Value = "text/plain", IsText = true)]
        ASC,

        [Metadata(Value = "video/x-ms-asf", IsBinary = true)]
        ASF,

        [Metadata(Value = "text/x-asm", IsText = true)]
        ASM,

        [Metadata(Value = "application/vnd.accpac.simply.aso", IsBinary = true)]
        ASO,

        [Metadata(Value = "video/x-ms-asf", IsBinary = true)]
        ASX,

        [Metadata(Value = "application/vnd.acucorp", IsBinary = true)]
        ATC,

        [Metadata(Value = "application/atom+xml", IsText = true)]
        ATOM,

        [Metadata(Value = "application/atomcat+xml", IsText = true)]
        ATOMCAT,

        [Metadata(Value = "application/atomsvc+xml", IsText = true)]
        ATOMSVC,

        [Metadata(Value = "application/vnd.antix.game-component", IsBinary = true)]
        ATX,

        [Metadata(Value = "audio/basic", IsBinary = true)]
        AU,

        [Metadata(Value = "video/x-msvideo", IsBinary = true)]
        AVI,

        [Metadata(Value = "application/applixware", IsBinary = true)]
        AW,

        [Metadata(Value = "application/vnd.airzip.filesecure.azf", IsBinary = true)]
        AZF,

        [Metadata(Value = "application/vnd.airzip.filesecure.azs", IsBinary = true)]
        AZS,

        [Metadata(Value = "application/vnd.amazon.ebook", IsBinary = true)]
        AZW,

        [Metadata(Value = "application/x-msdownload", IsBinary = true)]
        BAT,

        [Metadata(Value = "application/x-bcpio", IsBinary = true)]
        BCPIO,

        [Metadata(Value = "application/x-font-bdf", IsBinary = true)]
        BDF,

        [Metadata(Value = "application/vnd.syncml.dm+wbxml", IsText = true)]
        BDM,

        [Metadata(Value = "application/vnd.realvnc.bed", IsBinary = true)]
        BED,

        [Metadata(Value = "application/vnd.fujitsu.oasysprs", IsBinary = true)]
        BH2,

        [Metadata(Value = "application/octet-stream", IsBinary = true)]
        BIN,

        [Metadata(Value = "application/x-blorb", IsBinary = true)]
        BLB,

        [Metadata(Value = "application/x-blorb", IsBinary = true)]
        BLORB,

        [Metadata(Value = "application/vnd.bmi", IsBinary = true)]
        BMI,

        [Metadata(Value = "image/bmp", IsBinary = true)]
        BMP,

        [Metadata(Value = "application/vnd.framemaker", IsBinary = true)]
        BOOK,

        [Metadata(Value = "application/vnd.previewsystems.box", IsBinary = true)]
        BOX,

        [Metadata(Value = "application/x-bzip2", IsBinary = true)]
        BOZ,

        [Metadata(Value = "application/octet-stream", IsBinary = true)]
        BPK,

        [Metadata(Value = "image/prs.btif", IsBinary = true)]
        BTIF,

        [Metadata(Value = "application/x-bzip", IsBinary = true)]
        BZ,

        [Metadata(Value = "application/x-bzip2", IsBinary = true)]
        BZ2,

        [Metadata(Value = "text/x-c", IsText = true)]
        C,

        [Metadata(Value = "application/vnd.cluetrust.cartomobile-config", IsBinary = true)]
        C11AMC,

        [Metadata(Value = "application/vnd.cluetrust.cartomobile-config-pkg", IsBinary = true)]
        C11AMZ,

        [Metadata(Value = "application/vnd.clonk.c4group", IsBinary = true)]
        C4D,

        [Metadata(Value = "application/vnd.clonk.c4group", IsBinary = true)]
        C4F,

        [Metadata(Value = "application/vnd.clonk.c4group", IsBinary = true)]
        C4G,

        [Metadata(Value = "application/vnd.clonk.c4group", IsBinary = true)]
        C4P,

        [Metadata(Value = "application/vnd.clonk.c4group", IsBinary = true)]
        C4U,

        [Metadata(Value = "application/vnd.ms-cab-compressed", IsBinary = true)]
        CAB,

        [Metadata(Value = "audio/x-caf", IsBinary = true)]
        CAF,

        [Metadata(Value = "application/vnd.tcpdump.pcap", IsBinary = true)]
        CAP,

        [Metadata(Value = "application/vnd.curl.car", IsBinary = true)]
        CAR,

        [Metadata(Value = "application/vnd.ms-pki.seccat", IsBinary = true)]
        CAT,

        [Metadata(Value = "application/x-cbr", IsBinary = true)]
        CB7,

        [Metadata(Value = "application/x-cbr", IsBinary = true)]
        CBA,

        [Metadata(Value = "application/x-cbr", IsBinary = true)]
        CBR,

        [Metadata(Value = "application/x-cbr", IsBinary = true)]
        CBT,

        [Metadata(Value = "application/x-cbr", IsBinary = true)]
        CBZ,

        [Metadata(Value = "text/x-c", IsText = true)]
        CC,

        [Metadata(Value = "application/x-director", IsBinary = true)]
        CCT,

        [Metadata(Value = "application/ccxml+xml", IsText = true)]
        CCXML,

        [Metadata(Value = "application/vnd.contact.cmsg", IsBinary = true)]
        CDBCMSG,

        [Metadata(Value = "application/x-netcdf", IsBinary = true)]
        CDF,

        [Metadata(Value = "application/vnd.mediastation.cdkey", IsBinary = true)]
        CDKEY,

        [Metadata(Value = "application/cdmi-capability", IsBinary = true)]
        CDMIA,

        [Metadata(Value = "application/cdmi-container", IsBinary = true)]
        CDMIC,

        [Metadata(Value = "application/cdmi-domain", IsBinary = true)]
        CDMID,

        [Metadata(Value = "application/cdmi-object", IsBinary = true)]
        CDMIO,

        [Metadata(Value = "application/cdmi-queue", IsBinary = true)]
        CDMIQ,

        [Metadata(Value = "chemical/x-cdx", IsBinary = true)]
        CDX,

        [Metadata(Value = "application/vnd.chemdraw+xml", IsText = true)]
        CDXML,

        [Metadata(Value = "application/vnd.cinderella", IsBinary = true)]
        CDY,

        [Metadata(Value = "application/pkix-cert", IsBinary = true)]
        CER,

        [Metadata(Value = "application/x-cfs-compressed", IsBinary = true)]
        CFS,

        [Metadata(Value = "image/cgm", IsBinary = true)]
        CGM,

        [Metadata(Value = "application/x-chat", IsBinary = true)]
        CHAT,

        [Metadata(Value = "application/vnd.ms-htmlhelp", IsBinary = true)]
        CHM,

        [Metadata(Value = "application/vnd.kde.kchart", IsBinary = true)]
        CHRT,

        [Metadata(Value = "chemical/x-cif", IsBinary = true)]
        CIF,

        [Metadata(Value = "application/vnd.anser-web-certificate-issue-initiation", IsBinary = true)]
        CII,

        [Metadata(Value = "application/vnd.ms-artgalry", IsBinary = true)]
        CIL,

        [Metadata(Value = "application/vnd.claymore", IsBinary = true)]
        CLA,

        [Metadata(Value = "application/java-vm", IsBinary = true)]
        CLASS,

        [Metadata(Value = "application/vnd.crick.clicker.keyboard", IsBinary = true)]
        CLKK,

        [Metadata(Value = "application/vnd.crick.clicker.palette", IsBinary = true)]
        CLKP,

        [Metadata(Value = "application/vnd.crick.clicker.template", IsBinary = true)]
        CLKT,

        [Metadata(Value = "application/vnd.crick.clicker.wordbank", IsBinary = true)]
        CLKW,

        [Metadata(Value = "application/vnd.crick.clicker", IsBinary = true)]
        CLKX,

        [Metadata(Value = "application/x-msclip", IsBinary = true)]
        CLP,

        [Metadata(Value = "application/vnd.cosmocaller", IsBinary = true)]
        CMC,

        [Metadata(Value = "chemical/x-cmdf", IsBinary = true)]
        CMDF,

        [Metadata(Value = "chemical/x-cml", IsBinary = true)]
        CML,

        [Metadata(Value = "application/vnd.yellowriver-custom-menu", IsBinary = true)]
        CMP,

        [Metadata(Value = "image/x-cmx", IsBinary = true)]
        CMX,

        [Metadata(Value = "application/vnd.rim.cod", IsBinary = true)]
        COD,

        [Metadata(Value = "application/x-msdownload", IsBinary = true)]
        COM,

        [Metadata(Value = "text/plain", IsText = true)]
        CONF,

        [Metadata(Value = "application/x-cpio", IsBinary = true)]
        CPIO,

        [Metadata(Value = "text/x-c", IsText = true)]
        CPP,

        [Metadata(Value = "application/mac-compactpro", IsBinary = true)]
        CPT,

        [Metadata(Value = "application/x-mscardfile", IsBinary = true)]
        CRD,

        [Metadata(Value = "application/pkix-crl", IsBinary = true)]
        CRL,

        [Metadata(Value = "application/x-x509-ca-cert", IsBinary = true)]
        CRT,

        [Metadata(Value = "application/vnd.rig.cryptonote", IsBinary = true)]
        CRYPTONOTE,

        [Metadata(Value = "application/x-csh", IsBinary = true)]
        CSH,

        [Metadata(Value = "chemical/x-csml", IsBinary = true)]
        CSML,

        [Metadata(Value = "application/vnd.commonspace", IsBinary = true)]
        CSP,

        [Metadata(Value = "text/css", IsText = true)]
        CSS,

        [Metadata(Value = "application/x-director", IsBinary = true)]
        CST,

        [Metadata(Value = "text/csv", IsText = true)]
        CSV,

        [Metadata(Value = "application/cu-seeme", IsBinary = true)]
        CU,

        [Metadata(Value = "text/vnd.curl", IsText = true)]
        CURL,

        [Metadata(Value = "application/prs.cww", IsBinary = true)]
        CWW,

        [Metadata(Value = "application/x-director", IsBinary = true)]
        CXT,

        [Metadata(Value = "text/x-c", IsText = true)]
        CXX,

        [Metadata(Value = "model/vnd.collada+xml", IsText = true)]
        DAE,

        [Metadata(Value = "application/vnd.mobius.daf", IsBinary = true)]
        DAF,

        [Metadata(Value = "application/vnd.dart", IsBinary = true)]
        DART,

        [Metadata(Value = "application/vnd.fdsn.seed", IsBinary = true)]
        DATALESS,

        [Metadata(Value = "application/davmount+xml", IsText = true)]
        DAVMOUNT,

        [Metadata(Value = "application/docbook+xml", IsText = true)]
        DBK,

        [Metadata(Value = "application/x-director", IsBinary = true)]
        DCR,

        [Metadata(Value = "text/vnd.curl.dcurl", IsText = true)]
        DCURL,

        [Metadata(Value = "application/vnd.oma.dd2+xml", IsText = true)]
        DD2,

        [Metadata(Value = "application/vnd.fujixerox.ddd", IsBinary = true)]
        DDD,

        [Metadata(Value = "application/x-debian-package", IsBinary = true)]
        DEB,

        [Metadata(Value = "text/plain", IsText = true)]
        DEF,

        [Metadata(Value = "application/octet-stream", IsBinary = true)]
        DEPLOY,

        [Metadata(Value = "application/x-x509-ca-cert", IsBinary = true)]
        DER,

        [Metadata(Value = "application/vnd.dreamfactory", IsBinary = true)]
        DFAC,

        [Metadata(Value = "application/x-dgc-compressed", IsBinary = true)]
        DGC,

        [Metadata(Value = "text/x-c", IsText = true)]
        DIC,

        [Metadata(Value = "video/x-dv", IsBinary = true)]
        DIF,

        [Metadata(Value = "application/x-director", IsBinary = true)]
        DIR,

        [Metadata(Value = "application/vnd.mobius.dis", IsBinary = true)]
        DIS,

        [Metadata(Value = "application/octet-stream", IsBinary = true)]
        DIST,

        [Metadata(Value = "application/octet-stream", IsBinary = true)]
        DISTZ,

        [Metadata(Value = "image/vnd.djvu", IsBinary = true)]
        DJV,

        [Metadata(Value = "image/vnd.djvu", IsBinary = true)]
        DJVU,

        [Metadata(Value = "application/x-msdownload", IsBinary = true)]
        DLL,

        [Metadata(Value = "application/x-apple-diskimage", IsBinary = true)]
        DMG,

        [Metadata(Value = "application/vnd.tcpdump.pcap", IsBinary = true)]
        DMP,

        [Metadata(Value = "application/octet-stream", IsBinary = true)]
        DMS,

        [Metadata(Value = "application/vnd.dna", IsBinary = true)]
        DNA,

        [Metadata(Value = "application/msword", IsBinary = true)]
        DOC,

        [Metadata(Value = "application/vnd.ms-word.document.macroenabled.12", IsBinary = true)]
        DOCM,

        [Metadata(Value = "application/vnd.openxmlformats-officedocument.wordprocessingml.document", IsBinary = true)]
        DOCX,

        [Metadata(Value = "application/msword", IsBinary = true)]
        DOT,

        [Metadata(Value = "application/vnd.ms-word.template.macroenabled.12", IsBinary = true)]
        DOTM,

        [Metadata(Value = "application/vnd.openxmlformats-officedocument.wordprocessingml.template", IsBinary = true)]
        DOTX,

        [Metadata(Value = "application/vnd.osgi.dp", IsBinary = true)]
        DP,

        [Metadata(Value = "application/vnd.dpgraph", IsBinary = true)]
        DPG,

        [Metadata(Value = "audio/vnd.dra", IsBinary = true)]
        DRA,

        [Metadata(Value = "text/prs.lines.tag", IsText = true)]
        DSC,

        [Metadata(Value = "application/dssc+der", IsBinary = true)]
        DSSC,

        [Metadata(Value = "application/x-dtbook+xml", IsText = true)]
        DTB,

        [Metadata(Value = "application/xml-dtd", IsBinary = true)]
        DTD,

        [Metadata(Value = "audio/vnd.dts", IsBinary = true)]
        DTS,

        [Metadata(Value = "audio/vnd.dts.hd", IsBinary = true)]
        DTSHD,

        [Metadata(Value = "application/octet-stream", IsBinary = true)]
        DUMP,

        [Metadata(Value = "video/x-dv", IsBinary = true)]
        DV,

        [Metadata(Value = "video/vnd.dvb.file", IsBinary = true)]
        DVB,

        [Metadata(Value = "application/x-dvi", IsBinary = true)]
        DVI,

        [Metadata(Value = "model/vnd.dwf", IsBinary = true)]
        DWF,

        [Metadata(Value = "image/vnd.dwg", IsBinary = true)]
        DWG,

        [Metadata(Value = "image/vnd.dxf", IsBinary = true)]
        DXF,

        [Metadata(Value = "application/vnd.spotfire.dxp", IsBinary = true)]
        DXP,

        [Metadata(Value = "application/x-director", IsBinary = true)]
        DXR,

        [Metadata(Value = "audio/vnd.nuera.ecelp4800", IsBinary = true)]
        ECELP4800,

        [Metadata(Value = "audio/vnd.nuera.ecelp7470", IsBinary = true)]
        ECELP7470,

        [Metadata(Value = "audio/vnd.nuera.ecelp9600", IsBinary = true)]
        ECELP9600,

        [Metadata(Value = "application/ecmascript", IsBinary = true)]
        ECMA,

        [Metadata(Value = "application/vnd.novadigm.edm", IsBinary = true)]
        EDM,

        [Metadata(Value = "application/vnd.novadigm.edx", IsBinary = true)]
        EDX,

        [Metadata(Value = "application/vnd.picsel", IsBinary = true)]
        EFIF,

        [Metadata(Value = "application/vnd.pg.osasli", IsBinary = true)]
        EI6,

        [Metadata(Value = "application/octet-stream", IsBinary = true)]
        ELC,

        [Metadata(Value = "application/x-msmetafile", IsBinary = true)]
        EMF,

        [Metadata(Value = "message/rfc822", IsBinary = true)]
        EML,

        [Metadata(Value = "application/emma+xml", IsText = true)]
        EMMA,

        [Metadata(Value = "application/x-msmetafile", IsBinary = true)]
        EMZ,

        [Metadata(Value = "audio/vnd.digital-winds", IsBinary = true)]
        EOL,

        [Metadata(Value = "application/vnd.ms-fontobject", IsBinary = true)]
        EOT,

        [Metadata(Value = "application/postscript", IsBinary = true)]
        EPS,

        [Metadata(Value = "application/epub+zip", IsBinary = true)]
        EPUB,

        [Metadata(Value = "application/vnd.eszigno3+xml", IsText = true)]
        ES3,

        [Metadata(Value = "application/vnd.osgi.subsystem", IsBinary = true)]
        ESA,

        [Metadata(Value = "application/vnd.epson.esf", IsBinary = true)]
        ESF,

        [Metadata(Value = "application/vnd.eszigno3+xml", IsText = true)]
        ET3,

        [Metadata(Value = "text/x-setext", IsText = true)]
        ETX,

        [Metadata(Value = "application/x-eva", IsBinary = true)]
        EVA,

        [Metadata(Value = "application/x-envoy", IsBinary = true)]
        EVY,

        [Metadata(Value = "application/x-msdownload", IsBinary = true)]
        EXE,

        [Metadata(Value = "application/exi", IsBinary = true)]
        EXI,

        [Metadata(Value = "application/vnd.novadigm.ext", IsBinary = true)]
        EXT,

        [Metadata(Value = "MIME type (lowercased)", IsBinary = true)]
        EXTENSIONS,

        [Metadata(Value = "application/andrew-inset", IsBinary = true)]
        EZ,

        [Metadata(Value = "application/vnd.ezpix-album", IsBinary = true)]
        EZ2,

        [Metadata(Value = "application/vnd.ezpix-package", IsBinary = true)]
        EZ3,

        [Metadata(Value = "text/x-fortran", IsText = true)]
        F,

        [Metadata(Value = "video/x-f4v", IsBinary = true)]
        F4V,

        [Metadata(Value = "text/x-fortran", IsText = true)]
        F77,

        [Metadata(Value = "text/x-fortran", IsText = true)]
        F90,

        [Metadata(Value = "image/vnd.fastbidsheet", IsBinary = true)]
        FBS,

        [Metadata(Value = "application/vnd.adobe.formscentral.fcdt", IsBinary = true)]
        FCDT,

        [Metadata(Value = "application/vnd.isac.fcs", IsBinary = true)]
        FCS,

        [Metadata(Value = "application/vnd.fdf", IsBinary = true)]
        FDF,

        [Metadata(Value = "application/vnd.denovo.fcselayout-link", IsBinary = true)]
        FE_LAUNCH,

        [Metadata(Value = "application/vnd.fujitsu.oasysgp", IsBinary = true)]
        FG5,

        [Metadata(Value = "application/x-director", IsBinary = true)]
        FGD,

        [Metadata(Value = "image/x-freehand", IsBinary = true)]
        FH,

        [Metadata(Value = "image/x-freehand", IsBinary = true)]
        FH4,

        [Metadata(Value = "image/x-freehand", IsBinary = true)]
        FH5,

        [Metadata(Value = "image/x-freehand", IsBinary = true)]
        FH7,

        [Metadata(Value = "image/x-freehand", IsBinary = true)]
        FHC,

        [Metadata(Value = "application/x-xfig", IsBinary = true)]
        FIG,

        [Metadata(Value = "audio/x-flac", IsBinary = true)]
        FLAC,

        [Metadata(Value = "video/x-fli", IsBinary = true)]
        FLI,

        [Metadata(Value = "application/vnd.micrografx.flo", IsBinary = true)]
        FLO,

        [Metadata(Value = "video/x-flv", IsBinary = true)]
        FLV,

        [Metadata(Value = "application/vnd.kde.kivio", IsBinary = true)]
        FLW,

        [Metadata(Value = "text/vnd.fmi.flexstor", IsText = true)]
        FLX,

        [Metadata(Value = "text/vnd.fly", IsText = true)]
        FLY,

        [Metadata(Value = "application/vnd.framemaker", IsBinary = true)]
        FM,

        [Metadata(Value = "application/vnd.frogans.fnc", IsBinary = true)]
        FNC,

        [Metadata(Value = "text/x-fortran", IsText = true)]
        FOR,

        [Metadata(Value = "image/vnd.fpx", IsBinary = true)]
        FPX,

        [Metadata(Value = "application/vnd.framemaker", IsBinary = true)]
        FRAME,

        [Metadata(Value = "application/vnd.fsc.weblaunch", IsBinary = true)]
        FSC,

        [Metadata(Value = "image/vnd.fst", IsBinary = true)]
        FST,

        [Metadata(Value = "application/vnd.fluxtime.clip", IsBinary = true)]
        FTC,

        [Metadata(Value = "application/vnd.anser-web-funds-transfer-initiation", IsBinary = true)]
        FTI,

        [Metadata(Value = "video/vnd.fvt", IsBinary = true)]
        FVT,

        [Metadata(Value = "application/vnd.adobe.fxp", IsBinary = true)]
        FXP,

        [Metadata(Value = "application/vnd.adobe.fxp", IsBinary = true)]
        FXPL,

        [Metadata(Value = "application/vnd.fuzzysheet", IsBinary = true)]
        FZS,

        [Metadata(Value = "application/vnd.geoplan", IsBinary = true)]
        G2W,

        [Metadata(Value = "image/g3fax", IsBinary = true)]
        G3,

        [Metadata(Value = "application/vnd.geospace", IsBinary = true)]
        G3W,

        [Metadata(Value = "application/vnd.groove-account", IsBinary = true)]
        GAC,

        [Metadata(Value = "application/x-tads", IsBinary = true)]
        GAM,

        [Metadata(Value = "application/rpki-ghostbusters", IsBinary = true)]
        GBR,

        [Metadata(Value = "application/x-gca-compressed", IsBinary = true)]
        GCA,

        [Metadata(Value = "model/vnd.gdl", IsBinary = true)]
        GDL,

        [Metadata(Value = "application/vnd.dynageo", IsBinary = true)]
        GEO,

        [Metadata(Value = "application/vnd.geometry-explorer", IsBinary = true)]
        GEX,

        [Metadata(Value = "application/vnd.geogebra.file", IsBinary = true)]
        GGB,

        [Metadata(Value = "application/vnd.geogebra.tool", IsBinary = true)]
        GGT,

        [Metadata(Value = "application/vnd.groove-help", IsBinary = true)]
        GHF,

        [Metadata(Value = "image/gif", IsBinary = true)]
        GIF,

        [Metadata(Value = "application/vnd.groove-identity-message", IsBinary = true)]
        GIM,

        [Metadata(Value = "application/gml+xml", IsText = true)]
        GML,

        [Metadata(Value = "application/vnd.gmx", IsBinary = true)]
        GMX,

        [Metadata(Value = "application/x-gnumeric", IsBinary = true)]
        GNUMERIC,

        [Metadata(Value = "application/vnd.flographit", IsBinary = true)]
        GPH,

        [Metadata(Value = "application/gpx+xml", IsText = true)]
        GPX,

        [Metadata(Value = "application/vnd.grafeq", IsBinary = true)]
        GQF,

        [Metadata(Value = "application/vnd.grafeq", IsBinary = true)]
        GQS,

        [Metadata(Value = "application/srgs", IsBinary = true)]
        GRAM,

        [Metadata(Value = "application/x-gramps-xml", IsText = true)]
        GRAMPS,

        [Metadata(Value = "application/vnd.geometry-explorer", IsBinary = true)]
        GRE,

        [Metadata(Value = "application/vnd.groove-injector", IsBinary = true)]
        GRV,

        [Metadata(Value = "application/srgs+xml", IsText = true)]
        GRXML,

        [Metadata(Value = "application/x-font-ghostscript", IsBinary = true)]
        GSF,

        [Metadata(Value = "application/x-gtar", IsBinary = true)]
        GTAR,

        [Metadata(Value = "application/vnd.groove-tool-message", IsBinary = true)]
        GTM,

        [Metadata(Value = "model/vnd.gtw", IsBinary = true)]
        GTW,

        [Metadata(Value = "text/vnd.graphviz", IsText = true)]
        GV,

        [Metadata(Value = "application/gxf", IsBinary = true)]
        GXF,

        [Metadata(Value = "application/vnd.geonext", IsBinary = true)]
        GXT,

        [Metadata(Value = "text/x-c", IsText = true)]
        H,

        [Metadata(Value = "video/h261", IsBinary = true)]
        H261,

        [Metadata(Value = "video/h263", IsBinary = true)]
        H263,

        [Metadata(Value = "video/h264", IsBinary = true)]
        H264,

        [Metadata(Value = "application/vnd.hal+xml", IsText = true)]
        HAL,

        [Metadata(Value = "application/vnd.hbci", IsBinary = true)]
        HBCI,

        [Metadata(Value = "application/x-hdf", IsBinary = true)]
        HDF,

        [Metadata(Value = "text/x-c", IsText = true)]
        HH,

        [Metadata(Value = "application/winhlp", IsBinary = true)]
        HLP,

        [Metadata(Value = "application/vnd.hp-hpgl", IsBinary = true)]
        HPGL,

        [Metadata(Value = "application/vnd.hp-hpid", IsBinary = true)]
        HPID,

        [Metadata(Value = "application/vnd.hp-hps", IsBinary = true)]
        HPS,

        [Metadata(Value = "application/mac-binhex40", IsBinary = true)]
        HQX,

        [Metadata(Value = "application/vnd.kenameaapp", IsBinary = true)]
        HTKE,

        [Metadata(Value = "text/html", IsText = true)]
        HTM,

        [Metadata(Value = "text/html", IsText = true)]
        HTML,

        [Metadata(Value = "application/vnd.yamaha.hv-dic", IsBinary = true)]
        HVD,

        [Metadata(Value = "application/vnd.yamaha.hv-voice", IsBinary = true)]
        HVP,

        [Metadata(Value = "application/vnd.yamaha.hv-script", IsBinary = true)]
        HVS,

        [Metadata(Value = "application/vnd.intergeo", IsBinary = true)]
        I2G,

        [Metadata(Value = "x-conference/x-cooltalk", IsBinary = true)]
        IC,

        [Metadata(Value = "application/vnd.iccprofile", IsBinary = true)]
        ICC,

        [Metadata(Value = "x-conference/x-cooltalk", IsBinary = true)]
        ICE,

        [Metadata(Value = "application/vnd.iccprofile", IsBinary = true)]
        ICM,

        [Metadata(Value = "image/x-icon", IsBinary = true)]
        ICO,

        [Metadata(Value = "text/calendar", IsText = true)]
        ICS,

        [Metadata(Value = "image/ief", IsBinary = true)]
        IEF,

        [Metadata(Value = "text/calendar", IsText = true)]
        IFB,

        [Metadata(Value = "application/vnd.shana.informed.formdata", IsBinary = true)]
        IFM,

        [Metadata(Value = "model/iges", IsBinary = true)]
        IGES,

        [Metadata(Value = "application/vnd.igloader", IsBinary = true)]
        IGL,

        [Metadata(Value = "application/vnd.insors.igm", IsBinary = true)]
        IGM,

        [Metadata(Value = "model/iges", IsBinary = true)]
        IGS,

        [Metadata(Value = "application/vnd.micrografx.igx", IsBinary = true)]
        IGX,

        [Metadata(Value = "application/vnd.shana.informed.interchange", IsBinary = true)]
        IIF,

        [Metadata(Value = "application/vnd.accpac.simply.imp", IsBinary = true)]
        IMP,

        [Metadata(Value = "application/vnd.ms-ims", IsBinary = true)]
        IMS,

        [Metadata(Value = "text/plain", IsText = true)]
        IN,

        [Metadata(Value = "application/inkml+xml", IsText = true)]
        INK,

        [Metadata(Value = "application/inkml+xml", IsText = true)]
        INKML,

        [Metadata(Value = "application/x-install-instructions", IsBinary = true)]
        INSTALL,

        [Metadata(Value = "application/vnd.astraea-software.iota", IsBinary = true)]
        IOTA,

        [Metadata(Value = "application/ipfix", IsBinary = true)]
        IPFIX,

        [Metadata(Value = "application/vnd.shana.informed.package", IsBinary = true)]
        IPK,

        [Metadata(Value = "application/vnd.ibm.rights-management", IsBinary = true)]
        IRM,

        [Metadata(Value = "application/vnd.irepository.package+xml", IsText = true)]
        IRP,

        [Metadata(Value = "application/x-iso9660-image", IsBinary = true)]
        ISO,

        [Metadata(Value = "application/vnd.shana.informed.formtemplate", IsBinary = true)]
        ITP,

        [Metadata(Value = "application/vnd.immervision-ivp", IsBinary = true)]
        IVP,

        [Metadata(Value = "application/vnd.immervision-ivu", IsBinary = true)]
        IVU,

        [Metadata(Value = "text/vnd.sun.j2me.app-descriptor", IsText = true)]
        JAD,

        [Metadata(Value = "application/vnd.jam", IsBinary = true)]
        JAM,

        [Metadata(Value = "application/java-archive", IsBinary = true)]
        JAR,

        [Metadata(Value = "text/x-java-source", IsText = true)]
        JAVA,

        [Metadata(Value = "application/vnd.jisp", IsBinary = true)]
        JISP,

        [Metadata(Value = "application/vnd.hp-jlyt", IsBinary = true)]
        JLT,

        [Metadata(Value = "application/x-java-jnlp-file", IsBinary = true)]
        JNLP,

        [Metadata(Value = "application/vnd.joost.joda-archive", IsBinary = true)]
        JODA,

        [Metadata(Value = "image/jp2", IsBinary = true)]
        JP2,

        [Metadata(Value = "image/jpeg", IsBinary = true)]
        JPE,

        [Metadata(Value = "image/jpeg", IsBinary = true)]
        JPEG,

        [Metadata(Value = "image/jpeg", IsBinary = true)]
        JPG,

        [Metadata(Value = "video/jpm", IsBinary = true)]
        JPGM,

        [Metadata(Value = "video/jpeg", IsBinary = true)]
        JPGV,

        [Metadata(Value = "video/jpm", IsBinary = true)]
        JPM,

        [Metadata(Value = "application/javascript", IsText = true)]
        JS,

        [Metadata(Value = "application/json", IsText = true)]
        JSON,

        [Metadata(Value = "application/jsonml+json", IsText = true)]
        JSONML,

        [Metadata(Value = "audio/midi", IsBinary = true)]
        KAR,

        [Metadata(Value = "application/vnd.kde.karbon", IsBinary = true)]
        KARBON,

        [Metadata(Value = "application/vnd.kde.kformula", IsBinary = true)]
        KFO,

        [Metadata(Value = "application/vnd.kidspiration", IsBinary = true)]
        KIA,

        [Metadata(Value = "application/vnd.google-earth.kml+xml", IsText = true)]
        KML,

        [Metadata(Value = "application/vnd.google-earth.kmz", IsBinary = true)]
        KMZ,

        [Metadata(Value = "application/vnd.kinar", IsBinary = true)]
        KNE,

        [Metadata(Value = "application/vnd.kinar", IsBinary = true)]
        KNP,

        [Metadata(Value = "application/vnd.kde.kontour", IsBinary = true)]
        KON,

        [Metadata(Value = "application/vnd.kde.kpresenter", IsBinary = true)]
        KPR,

        [Metadata(Value = "application/vnd.kde.kpresenter", IsBinary = true)]
        KPT,

        [Metadata(Value = "application/vnd.ds-keypoint", IsBinary = true)]
        KPXX,

        [Metadata(Value = "application/vnd.kde.kspread", IsBinary = true)]
        KSP,

        [Metadata(Value = "application/vnd.kahootz", IsBinary = true)]
        KTR,

        [Metadata(Value = "image/ktx", IsBinary = true)]
        KTX,

        [Metadata(Value = "application/vnd.kahootz", IsBinary = true)]
        KTZ,

        [Metadata(Value = "application/vnd.kde.kword", IsBinary = true)]
        KWD,

        [Metadata(Value = "application/vnd.kde.kword", IsBinary = true)]
        KWT,

        [Metadata(Value = "application/vnd.las.las+xml", IsText = true)]
        LASXML,

        [Metadata(Value = "application/x-latex", IsBinary = true)]
        LATEX,

        [Metadata(Value = "application/vnd.llamagraphics.life-balance.desktop", IsBinary = true)]
        LBD,

        [Metadata(Value = "application/vnd.llamagraphics.life-balance.exchange+xml", IsText = true)]
        LBE,

        [Metadata(Value = "application/vnd.hhe.lesson-player", IsBinary = true)]
        LES,

        [Metadata(Value = "application/x-lzh-compressed", IsBinary = true)]
        LHA,

        [Metadata(Value = "application/vnd.route66.link66+xml", IsText = true)]
        LINK66,

        [Metadata(Value = "text/plain", IsText = true)]
        LIST,

        [Metadata(Value = "application/vnd.ibm.modcap", IsBinary = true)]
        LIST3820,

        [Metadata(Value = "application/vnd.ibm.modcap", IsBinary = true)]
        LISTAFP,

        [Metadata(Value = "application/x-ms-shortcut", IsBinary = true)]
        LNK,

        [Metadata(Value = "text/plain", IsText = true)]
        LOG,

        [Metadata(Value = "application/lost+xml", IsText = true)]
        LOSTXML,

        [Metadata(Value = "application/octet-stream", IsBinary = true)]
        LRF,

        [Metadata(Value = "application/vnd.ms-lrm", IsBinary = true)]
        LRM,

        [Metadata(Value = "application/vnd.frogans.ltf", IsBinary = true)]
        LTF,

        [Metadata(Value = "audio/vnd.lucent.voice", IsBinary = true)]
        LVP,

        [Metadata(Value = "application/vnd.lotus-wordpro", IsBinary = true)]
        LWP,

        [Metadata(Value = "application/x-lzh-compressed", IsBinary = true)]
        LZH,

        [Metadata(Value = "application/x-msmediaview", IsBinary = true)]
        M13,

        [Metadata(Value = "application/x-msmediaview", IsBinary = true)]
        M14,

        [Metadata(Value = "video/mpeg", IsBinary = true)]
        M1V,

        [Metadata(Value = "application/mp21", IsBinary = true)]
        M21,

        [Metadata(Value = "audio/mpeg", IsBinary = true)]
        M2A,

        [Metadata(Value = "video/mpeg", IsBinary = true)]
        M2V,

        [Metadata(Value = "audio/mpeg", IsBinary = true)]
        M3A,

        [Metadata(Value = "audio/x-mpegurl", IsBinary = true)]
        M3U,

        [Metadata(Value = "application/vnd.apple.mpegurl", IsBinary = true)]
        M3U8,

        [Metadata(Value = "audio/mp4a-latm", IsBinary = true)]
        M4A,

        [Metadata(Value = "audio/mp4a-latm", IsBinary = true)]
        M4B,

        [Metadata(Value = "audio/mp4a-latm", IsBinary = true)]
        M4P,

        [Metadata(Value = "video/vnd.mpegurl", IsBinary = true)]
        M4U,

        [Metadata(Value = "video/x-m4v", IsBinary = true)]
        M4V,

        [Metadata(Value = "application/mathematica", IsBinary = true)]
        MA,

        [Metadata(Value = "image/x-macpaint", IsBinary = true)]
        MAC,

        [Metadata(Value = "application/mads+xml", IsText = true)]
        MADS,

        [Metadata(Value = "application/vnd.ecowin.chart", IsBinary = true)]
        MAG,

        [Metadata(Value = "application/vnd.framemaker", IsBinary = true)]
        MAKER,

        [Metadata(Value = "application/x-troff-man", IsBinary = true)]
        MAN,

        [Metadata(Value = "application/octet-stream", IsBinary = true)]
        MAR,

        [Metadata(Value = "application/mathml+xml", IsText = true)]
        MATHML,

        [Metadata(Value = "application/mathematica", IsBinary = true)]
        MB,

        [Metadata(Value = "application/vnd.mobius.mbk", IsBinary = true)]
        MBK,

        [Metadata(Value = "application/mbox", IsBinary = true)]
        MBOX,

        [Metadata(Value = "application/vnd.medcalcdata", IsBinary = true)]
        MC1,

        [Metadata(Value = "application/vnd.mcd", IsBinary = true)]
        MCD,

        [Metadata(Value = "text/vnd.curl.mcurl", IsText = true)]
        MCURL,

        [Metadata(Value = "application/x-msaccess", IsBinary = true)]
        MDB,

        [Metadata(Value = "image/vnd.ms-modi", IsBinary = true)]
        MDI,

        [Metadata(Value = "application/x-troff-me", IsBinary = true)]
        ME,

        [Metadata(Value = "model/mesh", IsBinary = true)]
        MESH,

        [Metadata(Value = "application/metalink4+xml", IsText = true)]
        META4,

        [Metadata(Value = "application/metalink+xml", IsText = true)]
        METALINK,

        [Metadata(Value = "application/mets+xml", IsText = true)]
        METS,

        [Metadata(Value = "application/vnd.mfmp", IsBinary = true)]
        MFM,

        [Metadata(Value = "application/rpki-manifest", IsBinary = true)]
        MFT,

        [Metadata(Value = "application/vnd.osgeo.mapguide.package", IsBinary = true)]
        MGP,

        [Metadata(Value = "application/vnd.proteus.magazine", IsBinary = true)]
        MGZ,

        [Metadata(Value = "audio/midi", IsBinary = true)]
        MID,

        [Metadata(Value = "audio/midi", IsBinary = true)]
        MIDI,

        [Metadata(Value = "application/x-mie", IsBinary = true)]
        MIE,

        [Metadata(Value = "application/vnd.mif", IsBinary = true)]
        MIF,

        [Metadata(Value = "message/rfc822", IsBinary = true)]
        MIME,

        [Metadata(Value = "video/mj2", IsBinary = true)]
        MJ2,

        [Metadata(Value = "video/mj2", IsBinary = true)]
        MJP2,

        [Metadata(Value = "video/x-matroska", IsBinary = true)]
        MK3D,

        [Metadata(Value = "audio/x-matroska", IsBinary = true)]
        MKA,

        [Metadata(Value = "video/x-matroska", IsBinary = true)]
        MKS,

        [Metadata(Value = "video/x-matroska", IsBinary = true)]
        MKV,

        [Metadata(Value = "application/vnd.dolby.mlp", IsBinary = true)]
        MLP,

        [Metadata(Value = "application/vnd.chipnuts.karaoke-mmd", IsBinary = true)]
        MMD,

        [Metadata(Value = "application/vnd.smaf", IsBinary = true)]
        MMF,

        [Metadata(Value = "image/vnd.fujixerox.edmics-mmr", IsBinary = true)]
        MMR,

        [Metadata(Value = "video/x-mng", IsBinary = true)]
        MNG,

        [Metadata(Value = "application/x-msmoney", IsBinary = true)]
        MNY,

        [Metadata(Value = "application/x-mobipocket-ebook", IsBinary = true)]
        MOBI,

        [Metadata(Value = "application/mods+xml", IsText = true)]
        MODS,

        [Metadata(Value = "video/quicktime", IsBinary = true)]
        MOV,

        [Metadata(Value = "video/x-sgi-movie", IsBinary = true)]
        MOVIE,

        [Metadata(Value = "audio/mpeg", IsBinary = true)]
        MP2,

        [Metadata(Value = "application/mp21", IsBinary = true)]
        MP21,

        [Metadata(Value = "audio/mpeg", IsBinary = true)]
        MP2A,

        [Metadata(Value = "audio/mpeg", IsBinary = true)]
        MP3,

        [Metadata(Value = "video/mp4", IsBinary = true)]
        MP4,

        [Metadata(Value = "audio/mp4", IsBinary = true)]
        MP4A,

        [Metadata(Value = "application/mp4", IsBinary = true)]
        MP4S,

        [Metadata(Value = "video/mp4", IsBinary = true)]
        MP4V,

        [Metadata(Value = "application/vnd.mophun.certificate", IsBinary = true)]
        MPC,

        [Metadata(Value = "video/mpeg", IsBinary = true)]
        MPE,

        [Metadata(Value = "video/mpeg", IsBinary = true)]
        MPEG,

        [Metadata(Value = "video/mpeg", IsBinary = true)]
        MPG,

        [Metadata(Value = "video/mp4", IsBinary = true)]
        MPG4,

        [Metadata(Value = "audio/mpeg", IsBinary = true)]
        MPGA,

        [Metadata(Value = "application/vnd.apple.installer+xml", IsText = true)]
        MPKG,

        [Metadata(Value = "application/vnd.blueice.multipass", IsBinary = true)]
        MPM,

        [Metadata(Value = "application/vnd.mophun.application", IsBinary = true)]
        MPN,

        [Metadata(Value = "application/vnd.ms-project", IsBinary = true)]
        MPP,

        [Metadata(Value = "application/vnd.ms-project", IsBinary = true)]
        MPT,

        [Metadata(Value = "application/vnd.ibm.minipay", IsBinary = true)]
        MPY,

        [Metadata(Value = "application/vnd.mobius.mqy", IsBinary = true)]
        MQY,

        [Metadata(Value = "application/marc", IsBinary = true)]
        MRC,

        [Metadata(Value = "application/marcxml+xml", IsText = true)]
        MRCX,

        [Metadata(Value = "application/x-troff-ms", IsBinary = true)]
        MS,

        [Metadata(Value = "application/mediaservercontrol+xml", IsText = true)]
        MSCML,

        [Metadata(Value = "application/vnd.fdsn.mseed", IsBinary = true)]
        MSEED,

        [Metadata(Value = "application/vnd.mseq", IsBinary = true)]
        MSEQ,

        [Metadata(Value = "application/vnd.epson.msf", IsBinary = true)]
        MSF,

        [Metadata(Value = "model/mesh", IsBinary = true)]
        MSH,

        [Metadata(Value = "application/x-msdownload", IsBinary = true)]
        MSI,

        [Metadata(Value = "application/vnd.mobius.msl", IsBinary = true)]
        MSL,

        [Metadata(Value = "application/vnd.muvee.style", IsBinary = true)]
        MSTY,

        [Metadata(Value = "model/vnd.mts", IsBinary = true)]
        MTS,

        [Metadata(Value = "application/vnd.musician", IsBinary = true)]
        MUS,

        [Metadata(Value = "application/vnd.recordare.musicxml+xml", IsText = true)]
        MUSICXML,

        [Metadata(Value = "application/x-msmediaview", IsBinary = true)]
        MVB,

        [Metadata(Value = "application/vnd.mfer", IsBinary = true)]
        MWF,

        [Metadata(Value = "application/mxf", IsBinary = true)]
        MXF,

        [Metadata(Value = "application/vnd.recordare.musicxml", IsText = true)]
        MXL,

        [Metadata(Value = "application/xv+xml", IsText = true)]
        MXML,

        [Metadata(Value = "application/vnd.triscape.mxs", IsBinary = true)]
        MXS,

        [Metadata(Value = "video/vnd.mpegurl", IsBinary = true)]
        MXU,

        [Metadata(Value = "text/n3", IsText = true)]
        N3,

        [Metadata(Value = "application/mathematica", IsBinary = true)]
        NB,

        [Metadata(Value = "application/vnd.wolfram.player", IsBinary = true)]
        NBP,

        [Metadata(Value = "application/x-netcdf", IsBinary = true)]
        NC,

        [Metadata(Value = "application/x-dtbncx+xml", IsText = true)]
        NCX,

        [Metadata(Value = "text/x-nfo", IsText = true)]
        NFO,

        [Metadata(Value = "application/vnd.nokia.n-gage.data", IsBinary = true)]
        NGDAT,

        [Metadata(Value = "application/vnd.nitf", IsBinary = true)]
        NITF,

        [Metadata(Value = "application/vnd.neurolanguage.nlu", IsBinary = true)]
        NLU,

        [Metadata(Value = "application/vnd.enliven", IsBinary = true)]
        NML,

        [Metadata(Value = "application/vnd.noblenet-directory", IsBinary = true)]
        NND,

        [Metadata(Value = "application/vnd.noblenet-sealer", IsBinary = true)]
        NNS,

        [Metadata(Value = "application/vnd.noblenet-web", IsBinary = true)]
        NNW,

        [Metadata(Value = "image/vnd.net-fpx", IsBinary = true)]
        NPX,

        [Metadata(Value = "application/x-conference", IsBinary = true)]
        NSC,

        [Metadata(Value = "application/vnd.lotus-notes", IsBinary = true)]
        NSF,

        [Metadata(Value = "application/vnd.nitf", IsBinary = true)]
        NTF,

        [Metadata(Value = "application/x-nzb", IsBinary = true)]
        NZB,

        [Metadata(Value = "application/vnd.fujitsu.oasys2", IsBinary = true)]
        OA2,

        [Metadata(Value = "application/vnd.fujitsu.oasys3", IsBinary = true)]
        OA3,

        [Metadata(Value = "application/vnd.fujitsu.oasys", IsBinary = true)]
        OAS,

        [Metadata(Value = "application/x-msbinder", IsBinary = true)]
        OBD,

        [Metadata(Value = "application/x-tgif", IsBinary = true)]
        OBJ,

        [Metadata(Value = "application/oda", IsBinary = true)]
        ODA,

        [Metadata(Value = "application/vnd.oasis.opendocument.database", IsBinary = true)]
        ODB,

        [Metadata(Value = "application/vnd.oasis.opendocument.chart", IsBinary = true)]
        ODC,

        [Metadata(Value = "application/vnd.oasis.opendocument.formula", IsBinary = true)]
        ODF,

        [Metadata(Value = "application/vnd.oasis.opendocument.formula-template", IsBinary = true)]
        ODFT,

        [Metadata(Value = "application/vnd.oasis.opendocument.graphics", IsBinary = true)]
        ODG,

        [Metadata(Value = "application/vnd.oasis.opendocument.image", IsBinary = true)]
        ODI,

        [Metadata(Value = "application/vnd.oasis.opendocument.text-master", IsBinary = true)]
        ODM,

        [Metadata(Value = "application/vnd.oasis.opendocument.presentation", IsBinary = true)]
        ODP,

        [Metadata(Value = "application/vnd.oasis.opendocument.spreadsheet", IsBinary = true)]
        ODS,

        [Metadata(Value = "application/vnd.oasis.opendocument.text", IsBinary = true)]
        ODT,

        [Metadata(Value = "audio/ogg", IsBinary = true)]
        OGA,

        [Metadata(Value = "video/ogg", IsBinary = true)]
        OGG,

        [Metadata(Value = "video/ogg", IsBinary = true)]
        OGV,

        [Metadata(Value = "application/ogg", IsBinary = true)]
        OGX,

        [Metadata(Value = "application/omdoc+xml", IsText = true)]
        OMDOC,

        [Metadata(Value = "application/onenote", IsBinary = true)]
        ONEPKG,

        [Metadata(Value = "application/onenote", IsBinary = true)]
        ONETMP,

        [Metadata(Value = "application/onenote", IsBinary = true)]
        ONETOC,

        [Metadata(Value = "application/onenote", IsBinary = true)]
        ONETOC2,

        [Metadata(Value = "application/oebps-package+xml", IsText = true)]
        OPF,

        [Metadata(Value = "text/x-opml", IsText = true)]
        OPML,

        [Metadata(Value = "application/vnd.palm", IsBinary = true)]
        OPRC,

        [Metadata(Value = "application/vnd.lotus-organizer", IsBinary = true)]
        ORG,

        [Metadata(Value = "application/vnd.yamaha.openscoreformat", IsBinary = true)]
        OSF,

        [Metadata(Value = "application/vnd.yamaha.openscoreformat.osfpvg+xml", IsText = true)]
        OSFPVG,

        [Metadata(Value = "application/vnd.oasis.opendocument.chart-template", IsBinary = true)]
        OTC,

        [Metadata(Value = "application/x-font-otf", IsBinary = true)]
        OTF,

        [Metadata(Value = "application/vnd.oasis.opendocument.graphics-template", IsBinary = true)]
        OTG,

        [Metadata(Value = "application/vnd.oasis.opendocument.text-web", IsBinary = true)]
        OTH,

        [Metadata(Value = "application/vnd.oasis.opendocument.image-template", IsBinary = true)]
        OTI,

        [Metadata(Value = "application/vnd.oasis.opendocument.presentation-template", IsBinary = true)]
        OTP,

        [Metadata(Value = "application/vnd.oasis.opendocument.spreadsheet-template", IsBinary = true)]
        OTS,

        [Metadata(Value = "application/vnd.oasis.opendocument.text-template", IsBinary = true)]
        OTT,

        [Metadata(Value = "application/oxps", IsBinary = true)]
        OXPS,

        [Metadata(Value = "application/vnd.openofficeorg.extension", IsBinary = true)]
        OXT,

        [Metadata(Value = "text/x-pascal", IsText = true)]
        P,

        [Metadata(Value = "application/pkcs10", IsBinary = true)]
        P10,

        [Metadata(Value = "application/x-pkcs12", IsBinary = true)]
        P12,

        [Metadata(Value = "application/x-pkcs7-certificates", IsBinary = true)]
        P7B,

        [Metadata(Value = "application/pkcs7-mime", IsBinary = true)]
        P7C,

        [Metadata(Value = "application/pkcs7-mime", IsBinary = true)]
        P7M,

        [Metadata(Value = "application/x-pkcs7-certreqresp", IsBinary = true)]
        P7R,

        [Metadata(Value = "application/pkcs7-signature", IsBinary = true)]
        P7S,

        [Metadata(Value = "application/pkcs8", IsBinary = true)]
        P8,

        [Metadata(Value = "text/x-pascal", IsText = true)]
        PAS,

        [Metadata(Value = "application/vnd.pawaafile", IsBinary = true)]
        PAW,

        [Metadata(Value = "application/vnd.powerbuilder6", IsBinary = true)]
        PBD,

        [Metadata(Value = "image/x-portable-bitmap", IsBinary = true)]
        PBM,

        [Metadata(Value = "application/vnd.tcpdump.pcap", IsBinary = true)]
        PCAP,

        [Metadata(Value = "application/x-font-pcf", IsBinary = true)]
        PCF,

        [Metadata(Value = "application/vnd.hp-pcl", IsBinary = true)]
        PCL,

        [Metadata(Value = "application/vnd.hp-pclxl", IsBinary = true)]
        PCLXL,

        [Metadata(Value = "image/x-pict", IsBinary = true)]
        PCT,

        [Metadata(Value = "application/vnd.curl.pcurl", IsBinary = true)]
        PCURL,

        [Metadata(Value = "image/x-pcx", IsBinary = true)]
        PCX,

        [Metadata(Value = "applicaton/octet-stream", IsBinary = true)]
        PDB,

        [Metadata(Value = "application/pdf", IsBinary = true)]
        PDF,

        [Metadata(Value = "application/x-font-type1", IsBinary = true)]
        PFA,

        [Metadata(Value = "application/x-font-type1", IsBinary = true)]
        PFB,

        [Metadata(Value = "application/x-font-type1", IsBinary = true)]
        PFM,

        [Metadata(Value = "application/font-tdpfr", IsBinary = true)]
        PFR,

        [Metadata(Value = "application/x-pkcs12", IsBinary = true)]
        PFX,

        [Metadata(Value = "image/x-portable-graymap", IsBinary = true)]
        PGM,

        [Metadata(Value = "application/x-chess-pgn", IsBinary = true)]
        PGN,

        [Metadata(Value = "application/pgp-encrypted", IsBinary = true)]
        PGP,

        [Metadata(Value = "image/x-pict", IsBinary = true)]
        PIC,

        [Metadata(Value = "image/pict", IsBinary = true)]
        PICT,

        [Metadata(Value = "application/octet-stream", IsBinary = true)]
        PKG,

        [Metadata(Value = "application/pkixcmp", IsBinary = true)]
        PKI,

        [Metadata(Value = "application/pkix-pkipath", IsBinary = true)]
        PKIPATH,

        [Metadata(Value = "application/vnd.3gpp.pic-bw-large", IsBinary = true)]
        PLB,

        [Metadata(Value = "application/vnd.mobius.plc", IsBinary = true)]
        PLC,

        [Metadata(Value = "application/vnd.pocketlearn", IsBinary = true)]
        PLF,

        [Metadata(Value = "application/pls+xml", IsText = true)]
        PLS,

        [Metadata(Value = "application/vnd.ctc-posml", IsBinary = true)]
        PML,

        [Metadata(Value = "image/png", IsBinary = true)]
        PNG,

        [Metadata(Value = "image/x-portable-anymap", IsBinary = true)]
        PNM,

        [Metadata(Value = "image/x-macpaint", IsBinary = true)]
        PNT,

        [Metadata(Value = "image/x-macpaint", IsBinary = true)]
        PNTG,

        [Metadata(Value = "application/vnd.macports.portpkg", IsBinary = true)]
        PORTPKG,

        [Metadata(Value = "application/vnd.ms-powerpoint", IsBinary = true)]
        POT,

        [Metadata(Value = "application/vnd.ms-powerpoint.template.macroenabled.12", IsBinary = true)]
        POTM,

        [Metadata(Value = "application/vnd.openxmlformats-officedocument.presentationml.template", IsBinary = true)]
        POTX,

        [Metadata(Value = "application/vnd.ms-powerpoint.addin.macroenabled.12", IsBinary = true)]
        PPAM,

        [Metadata(Value = "application/vnd.cups-ppd", IsBinary = true)]
        PPD,

        [Metadata(Value = "image/x-portable-pixmap", IsBinary = true)]
        PPM,

        [Metadata(Value = "application/vnd.ms-powerpoint", IsBinary = true)]
        PPS,

        [Metadata(Value = "application/vnd.ms-powerpoint.slideshow.macroenabled.12", IsBinary = true)]
        PPSM,

        [Metadata(Value = "application/vnd.openxmlformats-officedocument.presentationml.slideshow", IsBinary = true)]
        PPSX,

        [Metadata(Value = "application/vnd.ms-powerpoint", IsBinary = true)]
        PPT,

        [Metadata(Value = "application/vnd.ms-powerpoint.presentation.macroenabled.12", IsBinary = true)]
        PPTM,

        [Metadata(Value = "application/vnd.openxmlformats-officedocument.presentationml.presentation", IsBinary = true)]
        PPTX,

        [Metadata(Value = "application/vnd.palm", IsBinary = true)]
        PQA,

        [Metadata(Value = "application/x-mobipocket-ebook", IsBinary = true)]
        PRC,

        [Metadata(Value = "application/vnd.lotus-freelance", IsBinary = true)]
        PRE,

        [Metadata(Value = "application/pics-rules", IsBinary = true)]
        PRF,

        [Metadata(Value = "application/postscript", IsBinary = true)]
        PS,

        [Metadata(Value = "application/vnd.3gpp.pic-bw-small", IsBinary = true)]
        PSB,

        [Metadata(Value = "image/vnd.adobe.photoshop", IsBinary = true)]
        PSD,

        [Metadata(Value = "application/x-font-linux-psf", IsBinary = true)]
        PSF,

        [Metadata(Value = "application/pskc+xml", IsText = true)]
        PSKCXML,

        [Metadata(Value = "application/vnd.pvi.ptid1", IsBinary = true)]
        PTID,

        [Metadata(Value = "application/x-mspublisher", IsBinary = true)]
        PUB,

        [Metadata(Value = "application/vnd.3gpp.pic-bw-var", IsBinary = true)]
        PVB,

        [Metadata(Value = "application/vnd.3m.post-it-notes", IsBinary = true)]
        PWN,

        [Metadata(Value = "audio/vnd.ms-playready.media.pya", IsBinary = true)]
        PYA,

        [Metadata(Value = "video/vnd.ms-playready.media.pyv", IsBinary = true)]
        PYV,

        [Metadata(Value = "application/vnd.epson.quickanime", IsBinary = true)]
        QAM,

        [Metadata(Value = "application/vnd.intu.qbo", IsBinary = true)]
        QBO,

        [Metadata(Value = "application/vnd.intu.qfx", IsBinary = true)]
        QFX,

        [Metadata(Value = "application/vnd.publishare-delta-tree", IsBinary = true)]
        QPS,

        [Metadata(Value = "video/quicktime", IsBinary = true)]
        QT,

        [Metadata(Value = "image/x-quicktime", IsBinary = true)]
        QTI,

        [Metadata(Value = "image/x-quicktime", IsBinary = true)]
        QTIF,

        [Metadata(Value = "application/vnd.quark.quarkxpress", IsBinary = true)]
        QWD,

        [Metadata(Value = "application/vnd.quark.quarkxpress", IsBinary = true)]
        QWT,

        [Metadata(Value = "application/vnd.quark.quarkxpress", IsBinary = true)]
        QXB,

        [Metadata(Value = "application/vnd.quark.quarkxpress", IsBinary = true)]
        QXD,

        [Metadata(Value = "application/vnd.quark.quarkxpress", IsBinary = true)]
        QXL,

        [Metadata(Value = "application/vnd.quark.quarkxpress", IsBinary = true)]
        QXT,

        [Metadata(Value = "audio/x-pn-realaudio", IsBinary = true)]
        RA,

        [Metadata(Value = "audio/x-pn-realaudio", IsBinary = true)]
        RAM,

        [Metadata(Value = "application/x-rar-compressed", IsBinary = true)]
        RAR,

        [Metadata(Value = "image/x-cmu-raster", IsBinary = true)]
        RAS,

        [Metadata(Value = "application/vnd.ipunplugged.rcprofile", IsBinary = true)]
        RCPROFILE,

        [Metadata(Value = "application/rdf+xml", IsText = true)]
        RDF,

        [Metadata(Value = "application/vnd.data-vision.rdz", IsBinary = true)]
        RDZ,

        [Metadata(Value = "application/vnd.businessobjects", IsBinary = true)]
        REP,

        [Metadata(Value = "application/x-dtbresource+xml", IsText = true)]
        RES,

        [Metadata(Value = "image/x-rgb", IsBinary = true)]
        RGB,

        [Metadata(Value = "application/reginfo+xml", IsText = true)]
        RIF,

        [Metadata(Value = "audio/vnd.rip", IsBinary = true)]
        RIP,

        [Metadata(Value = "application/x-research-info-systems", IsBinary = true)]
        RIS,

        [Metadata(Value = "application/resource-lists+xml", IsText = true)]
        RL,

        [Metadata(Value = "image/vnd.fujixerox.edmics-rlc", IsBinary = true)]
        RLC,

        [Metadata(Value = "application/resource-lists-diff+xml", IsText = true)]
        RLD,

        [Metadata(Value = "application/vnd.rn-realmedia", IsBinary = true)]
        RM,

        [Metadata(Value = "audio/midi", IsBinary = true)]
        RMI,

        [Metadata(Value = "audio/x-pn-realaudio-plugin", IsBinary = true)]
        RMP,

        [Metadata(Value = "application/vnd.jcp.javame.midlet-rms", IsBinary = true)]
        RMS,

        [Metadata(Value = "application/vnd.rn-realmedia-vbr", IsBinary = true)]
        RMVB,

        [Metadata(Value = "application/relax-ng-compact-syntax", IsBinary = true)]
        RNC,

        [Metadata(Value = "application/rpki-roa", IsBinary = true)]
        ROA,

        [Metadata(Value = "application/x-troff", IsBinary = true)]
        ROFF,

        [Metadata(Value = "application/vnd.cloanto.rp9", IsBinary = true)]
        RP9,

        [Metadata(Value = "application/vnd.nokia.radio-presets", IsBinary = true)]
        RPSS,

        [Metadata(Value = "application/vnd.nokia.radio-preset", IsBinary = true)]
        RPST,

        [Metadata(Value = "application/sparql-query", IsBinary = true)]
        RQ,

        [Metadata(Value = "application/rls-services+xml", IsText = true)]
        RS,

        [Metadata(Value = "application/rsd+xml", IsText = true)]
        RSD,

        [Metadata(Value = "application/rss+xml", IsText = true)]
        RSS,

        [Metadata(Value = "application/rtf", IsBinary = true)]
        RTF,

        [Metadata(Value = "text/richtext", IsText = true)]
        RTX,

        [Metadata(Value = "text/x-asm", IsText = true)]
        S,

        [Metadata(Value = "audio/s3m", IsBinary = true)]
        S3M,

        [Metadata(Value = "application/vnd.yamaha.smaf-audio", IsBinary = true)]
        SAF,

        [Metadata(Value = "application/sbml+xml", IsText = true)]
        SBML,

        [Metadata(Value = "application/vnd.ibm.secure-container", IsBinary = true)]
        SC,

        [Metadata(Value = "application/x-msschedule", IsBinary = true)]
        SCD,

        [Metadata(Value = "application/vnd.lotus-screencam", IsBinary = true)]
        SCM,

        [Metadata(Value = "application/scvp-cv-request", IsBinary = true)]
        SCQ,

        [Metadata(Value = "application/scvp-cv-response", IsBinary = true)]
        SCS,

        [Metadata(Value = "text/vnd.curl.scurl", IsText = true)]
        SCURL,

        [Metadata(Value = "application/vnd.stardivision.draw", IsBinary = true)]
        SDA,

        [Metadata(Value = "application/vnd.stardivision.calc", IsBinary = true)]
        SDC,

        [Metadata(Value = "application/vnd.stardivision.impress", IsBinary = true)]
        SDD,

        [Metadata(Value = "application/vnd.solent.sdkm+xml", IsText = true)]
        SDKD,

        [Metadata(Value = "application/vnd.solent.sdkm+xml", IsText = true)]
        SDKM,

        [Metadata(Value = "application/sdp", IsBinary = true)]
        SDP,

        [Metadata(Value = "application/vnd.stardivision.writer", IsBinary = true)]
        SDW,

        [Metadata(Value = "application/vnd.seemail", IsBinary = true)]
        SEE,

        [Metadata(Value = "application/vnd.fdsn.seed", IsBinary = true)]
        SEED,

        [Metadata(Value = "application/vnd.sema", IsBinary = true)]
        SEMA,

        [Metadata(Value = "application/vnd.semd", IsBinary = true)]
        SEMD,

        [Metadata(Value = "application/vnd.semf", IsBinary = true)]
        SEMF,

        [Metadata(Value = "application/java-serialized-object", IsBinary = true)]
        SER,

        [Metadata(Value = "application/set-payment-initiation", IsBinary = true)]
        SETPAY,

        [Metadata(Value = "application/set-registration-initiation", IsBinary = true)]
        SETREG,

        [Metadata(Value = "application/vnd.spotfire.sfs", IsBinary = true)]
        SFS,

        [Metadata(Value = "text/x-sfv", IsText = true)]
        SFV,

        [Metadata(Value = "image/sgi", IsBinary = true)]
        SGI,

        [Metadata(Value = "application/vnd.stardivision.writer-global", IsBinary = true)]
        SGL,

        [Metadata(Value = "text/sgml", IsText = true)]
        SGM,

        [Metadata(Value = "text/sgml", IsText = true)]
        SGML,

        [Metadata(Value = "application/x-sh", IsBinary = true)]
        SH,

        [Metadata(Value = "application/x-shar", IsBinary = true)]
        SHAR,

        [Metadata(Value = "application/shf+xml", IsText = true)]
        SHF,

        [Metadata(Value = "image/x-mrsid-image", IsBinary = true)]
        SID,

        [Metadata(Value = "application/pgp-signature", IsBinary = true)]
        SIG,

        [Metadata(Value = "audio/silk", IsBinary = true)]
        SIL,

        [Metadata(Value = "model/mesh", IsBinary = true)]
        SILO,

        [Metadata(Value = "application/vnd.symbian.install", IsBinary = true)]
        SIS,

        [Metadata(Value = "application/vnd.symbian.install", IsBinary = true)]
        SISX,

        [Metadata(Value = "application/x-stuffit", IsBinary = true)]
        SIT,

        [Metadata(Value = "application/x-stuffitx", IsBinary = true)]
        SITX,

        [Metadata(Value = "application/x-koan", IsBinary = true)]
        SKD,

        [Metadata(Value = "application/x-koan", IsBinary = true)]
        SKM,

        [Metadata(Value = "application/x-koan", IsBinary = true)]
        SKP,

        [Metadata(Value = "application/x-koan", IsBinary = true)]
        SKT,

        [Metadata(Value = "application/vnd.ms-powerpoint.slide.macroenabled.12", IsBinary = true)]
        SLDM,

        [Metadata(Value = "application/vnd.openxmlformats-officedocument.presentationml.slide", IsBinary = true)]
        SLDX,

        [Metadata(Value = "application/vnd.epson.salt", IsBinary = true)]
        SLT,

        [Metadata(Value = "application/vnd.stepmania.stepchart", IsBinary = true)]
        SM,

        [Metadata(Value = "application/vnd.stardivision.math", IsBinary = true)]
        SMF,

        [Metadata(Value = "application/smil+xml", IsText = true)]
        SMI,

        [Metadata(Value = "application/smil+xml", IsText = true)]
        SMIL,

        [Metadata(Value = "video/x-smv", IsBinary = true)]
        SMV,

        [Metadata(Value = "application/vnd.stepmania.package", IsBinary = true)]
        SMZIP,

        [Metadata(Value = "audio/basic", IsBinary = true)]
        SND,

        [Metadata(Value = "application/x-font-snf", IsBinary = true)]
        SNF,

        [Metadata(Value = "application/octet-stream", IsBinary = true)]
        SO,

        [Metadata(Value = "application/x-pkcs7-certificates", IsBinary = true)]
        SPC,

        [Metadata(Value = "application/vnd.yamaha.smaf-phrase", IsBinary = true)]
        SPF,

        [Metadata(Value = "application/x-futuresplash", IsBinary = true)]
        SPL,

        [Metadata(Value = "text/vnd.in3d.spot", IsText = true)]
        SPOT,

        [Metadata(Value = "application/scvp-vp-response", IsBinary = true)]
        SPP,

        [Metadata(Value = "application/scvp-vp-request", IsBinary = true)]
        SPQ,

        [Metadata(Value = "audio/ogg", IsBinary = true)]
        SPX,

        [Metadata(Value = "application/x-sql", IsBinary = true)]
        SQL,

        [Metadata(Value = "application/x-wais-source", IsBinary = true)]
        SRC,

        [Metadata(Value = "application/x-subrip", IsBinary = true)]
        SRT,

        [Metadata(Value = "application/sru+xml", IsText = true)]
        SRU,

        [Metadata(Value = "application/sparql-results+xml", IsText = true)]
        SRX,

        [Metadata(Value = "application/ssdl+xml", IsText = true)]
        SSDL,

        [Metadata(Value = "application/vnd.kodak-descriptor", IsBinary = true)]
        SSE,

        [Metadata(Value = "application/vnd.epson.ssf", IsBinary = true)]
        SSF,

        [Metadata(Value = "application/ssml+xml", IsText = true)]
        SSML,

        [Metadata(Value = "application/vnd.sailingtracker.track", IsBinary = true)]
        ST,

        [Metadata(Value = "application/vnd.sun.xml.calc.template", IsBinary = true)]
        STC,

        [Metadata(Value = "application/vnd.sun.xml.draw.template", IsBinary = true)]
        STD,

        [Metadata(Value = "application/vnd.wt.stf", IsBinary = true)]
        STF,

        [Metadata(Value = "application/vnd.sun.xml.impress.template", IsBinary = true)]
        STI,

        [Metadata(Value = "application/hyperstudio", IsBinary = true)]
        STK,

        [Metadata(Value = "application/vnd.ms-pki.stl", IsBinary = true)]
        STL,

        [Metadata(Value = "application/vnd.pg.format", IsBinary = true)]
        STR,

        [Metadata(Value = "application/vnd.sun.xml.writer.template", IsBinary = true)]
        STW,

        [Metadata(Value = "text/vnd.dvb.subtitle", IsText = true)]
        SUB,

        [Metadata(Value = "application/vnd.sus-calendar", IsBinary = true)]
        SUS,

        [Metadata(Value = "application/vnd.sus-calendar", IsBinary = true)]
        SUSP,

        [Metadata(Value = "application/x-sv4cpio", IsBinary = true)]
        SV4CPIO,

        [Metadata(Value = "application/x-sv4crc", IsBinary = true)]
        SV4CRC,

        [Metadata(Value = "application/vnd.dvb.service", IsBinary = true)]
        SVC,

        [Metadata(Value = "application/vnd.svd", IsBinary = true)]
        SVD,

        [Metadata(Value = "image/svg+xml", IsText = true)]
        SVG,

        [Metadata(Value = "image/svg+xml", IsText = true)]
        SVGZ,

        [Metadata(Value = "application/x-director", IsBinary = true)]
        SWA,

        [Metadata(Value = "application/x-shockwave-flash", IsBinary = true)]
        SWF,

        [Metadata(Value = "application/vnd.aristanetworks.swi", IsBinary = true)]
        SWI,

        [Metadata(Value = "application/vnd.sun.xml.calc", IsBinary = true)]
        SXC,

        [Metadata(Value = "application/vnd.sun.xml.draw", IsBinary = true)]
        SXD,

        [Metadata(Value = "application/vnd.sun.xml.writer.global", IsBinary = true)]
        SXG,

        [Metadata(Value = "application/vnd.sun.xml.impress", IsBinary = true)]
        SXI,

        [Metadata(Value = "application/vnd.sun.xml.math", IsBinary = true)]
        SXM,

        [Metadata(Value = "application/vnd.sun.xml.writer", IsBinary = true)]
        SXW,

        [Metadata(Value = "application/x-troff", IsBinary = true)]
        T,

        [Metadata(Value = "application/x-t3vm-image", IsBinary = true)]
        T3,

        [Metadata(Value = "application/vnd.mynfc", IsBinary = true)]
        TAGLET,

        [Metadata(Value = "application/vnd.tao.intent-module-archive", IsBinary = true)]
        TAO,

        [Metadata(Value = "application/x-tar", IsBinary = true)]
        TAR,

        [Metadata(Value = "application/vnd.3gpp2.tcap", IsBinary = true)]
        TCAP,

        [Metadata(Value = "application/x-tcl", IsBinary = true)]
        TCL,

        [Metadata(Value = "application/vnd.smart.teacher", IsBinary = true)]
        TEACHER,

        [Metadata(Value = "application/tei+xml", IsText = true)]
        TEI,

        [Metadata(Value = "application/tei+xml", IsText = true)]
        TEICORPUS,

        [Metadata(Value = "application/x-tex", IsBinary = true)]
        TEX,

        [Metadata(Value = "application/x-texinfo", IsBinary = true)]
        TEXI,

        [Metadata(Value = "application/x-texinfo", IsBinary = true)]
        TEXINFO,

        [Metadata(Value = "text/plain", IsText = true)]
        TEXT,

        [Metadata(Value = "application/thraud+xml", IsText = true)]
        TFI,

        [Metadata(Value = "application/x-tex-tfm", IsBinary = true)]
        TFM,

        [Metadata(Value = "image/x-tga", IsBinary = true)]
        TGA,

        [Metadata(Value = "application/vnd.ms-officetheme", IsBinary = true)]
        THMX,

        [Metadata(Value = "image/tiff", IsBinary = true)]
        TIF,

        [Metadata(Value = "image/tiff", IsBinary = true)]
        TIFF,

        [Metadata(Value = "application/vnd.tmobile-livetv", IsBinary = true)]
        TMO,

        [Metadata(Value = "application/x-bittorrent", IsBinary = true)]
        TORRENT,

        [Metadata(Value = "application/vnd.groove-tool-template", IsBinary = true)]
        TPL,

        [Metadata(Value = "application/vnd.trid.tpt", IsBinary = true)]
        TPT,

        [Metadata(Value = "application/x-troff", IsBinary = true)]
        TR,

        [Metadata(Value = "application/vnd.trueapp", IsBinary = true)]
        TRA,

        [Metadata(Value = "application/x-msterminal", IsBinary = true)]
        TRM,

        [Metadata(Value = "application/timestamped-data", IsBinary = true)]
        TSD,

        [Metadata(Value = "text/tab-separated-values", IsText = true)]
        TSV,

        [Metadata(Value = "application/x-font-ttf", IsBinary = true)]
        TTC,

        [Metadata(Value = "application/x-font-ttf", IsBinary = true)]
        TTF,

        [Metadata(Value = "text/turtle", IsText = true)]
        TTL,

        [Metadata(Value = "application/vnd.simtech-mindmapper", IsBinary = true)]
        TWD,

        [Metadata(Value = "application/vnd.simtech-mindmapper", IsBinary = true)]
        TWDS,

        [Metadata(Value = "application/vnd.genomatix.tuxedo", IsBinary = true)]
        TXD,

        [Metadata(Value = "application/vnd.mobius.txf", IsBinary = true)]
        TXF,

        [Metadata(Value = "text/plain", IsText = true)]
        TXT,

        [Metadata(Value = "application/x-authorware-bin", IsBinary = true)]
        U32,

        [Metadata(Value = "application/x-debian-package", IsBinary = true)]
        UDEB,

        [Metadata(Value = "application/vnd.ufdl", IsBinary = true)]
        UFD,

        [Metadata(Value = "application/vnd.ufdl", IsBinary = true)]
        UFDL,

        [Metadata(Value = "application/x-glulx", IsBinary = true)]
        ULX,

        [Metadata(Value = "application/vnd.umajin", IsBinary = true)]
        UMJ,

        [Metadata(Value = "application/vnd.unity", IsBinary = true)]
        UNITYWEB,

        [Metadata(Value = "application/vnd.uoml+xml", IsText = true)]
        UOML,

        [Metadata(Value = "text/uri-list", IsText = true)]
        URI,

        [Metadata(Value = "text/uri-list", IsText = true)]
        URIS,

        [Metadata(Value = "text/uri-list", IsText = true)]
        URLS,

        [Metadata(Value = "application/x-ustar", IsBinary = true)]
        USTAR,

        [Metadata(Value = "application/vnd.uiq.theme", IsBinary = true)]
        UTZ,

        [Metadata(Value = "text/x-uuencode", IsText = true)]
        UU,

        [Metadata(Value = "audio/vnd.dece.audio", IsBinary = true)]
        UVA,

        [Metadata(Value = "application/vnd.dece.data", IsBinary = true)]
        UVD,

        [Metadata(Value = "application/vnd.dece.data", IsBinary = true)]
        UVF,

        [Metadata(Value = "image/vnd.dece.graphic", IsBinary = true)]
        UVG,

        [Metadata(Value = "video/vnd.dece.hd", IsBinary = true)]
        UVH,

        [Metadata(Value = "image/vnd.dece.graphic", IsBinary = true)]
        UVI,

        [Metadata(Value = "video/vnd.dece.mobile", IsBinary = true)]
        UVM,

        [Metadata(Value = "video/vnd.dece.pd", IsBinary = true)]
        UVP,

        [Metadata(Value = "video/vnd.dece.sd", IsBinary = true)]
        UVS,

        [Metadata(Value = "application/vnd.dece.ttml+xml", IsText = true)]
        UVT,

        [Metadata(Value = "video/vnd.uvvu.mp4", IsBinary = true)]
        UVU,

        [Metadata(Value = "video/vnd.dece.video", IsBinary = true)]
        UVV,

        [Metadata(Value = "audio/vnd.dece.audio", IsBinary = true)]
        UVVA,

        [Metadata(Value = "application/vnd.dece.data", IsBinary = true)]
        UVVD,

        [Metadata(Value = "application/vnd.dece.data", IsBinary = true)]
        UVVF,

        [Metadata(Value = "image/vnd.dece.graphic", IsBinary = true)]
        UVVG,

        [Metadata(Value = "video/vnd.dece.hd", IsBinary = true)]
        UVVH,

        [Metadata(Value = "image/vnd.dece.graphic", IsBinary = true)]
        UVVI,

        [Metadata(Value = "video/vnd.dece.mobile", IsBinary = true)]
        UVVM,

        [Metadata(Value = "video/vnd.dece.pd", IsBinary = true)]
        UVVP,

        [Metadata(Value = "video/vnd.dece.sd", IsBinary = true)]
        UVVS,

        [Metadata(Value = "application/vnd.dece.ttml+xml", IsText = true)]
        UVVT,

        [Metadata(Value = "video/vnd.uvvu.mp4", IsBinary = true)]
        UVVU,

        [Metadata(Value = "video/vnd.dece.video", IsBinary = true)]
        UVVV,

        [Metadata(Value = "application/vnd.dece.unspecified", IsBinary = true)]
        UVVX,

        [Metadata(Value = "application/vnd.dece.zip", IsBinary = true)]
        UVVZ,

        [Metadata(Value = "application/vnd.dece.unspecified", IsBinary = true)]
        UVX,

        [Metadata(Value = "application/vnd.dece.zip", IsBinary = true)]
        UVZ,

        [Metadata(Value = "text/vcard", IsText = true)]
        VCARD,

        [Metadata(Value = "application/x-cdlink", IsBinary = true)]
        VCD,

        [Metadata(Value = "text/x-vcard", IsText = true)]
        VCF,

        [Metadata(Value = "application/vnd.groove-vcard", IsBinary = true)]
        VCG,

        [Metadata(Value = "text/x-vcalendar", IsText = true)]
        VCS,

        [Metadata(Value = "application/vnd.vcx", IsBinary = true)]
        VCX,

        [Metadata(Value = "application/vnd.visionary", IsBinary = true)]
        VIS,

        [Metadata(Value = "video/vnd.vivo", IsBinary = true)]
        VIV,

        [Metadata(Value = "video/x-ms-vob", IsBinary = true)]
        VOB,

        [Metadata(Value = "application/vnd.stardivision.writer", IsBinary = true)]
        VOR,

        [Metadata(Value = "application/x-authorware-bin", IsBinary = true)]
        VOX,

        [Metadata(Value = "model/vrml", IsBinary = true)]
        VRML,

        [Metadata(Value = "application/vnd.visio", IsBinary = true)]
        VSD,

        [Metadata(Value = "application/vnd.vsf", IsBinary = true)]
        VSF,

        [Metadata(Value = "application/vnd.visio", IsBinary = true)]
        VSS,

        [Metadata(Value = "application/vnd.visio", IsBinary = true)]
        VST,

        [Metadata(Value = "application/vnd.visio", IsBinary = true)]
        VSW,

        [Metadata(Value = "model/vnd.vtu", IsBinary = true)]
        VTU,

        [Metadata(Value = "application/voicexml+xml", IsText = true)]
        VXML,

        [Metadata(Value = "application/x-director", IsBinary = true)]
        W3D,

        [Metadata(Value = "application/x-doom", IsBinary = true)]
        WAD,

        [Metadata(Value = "audio/x-wav", IsBinary = true)]
        WAV,

        [Metadata(Value = "audio/x-ms-wax", IsBinary = true)]
        WAX,

        [Metadata(Value = "image/vnd.wap.wbmp", IsBinary = true)]
        WBMP,

        [Metadata(Value = "application/vnd.wap.wbxml", IsText = true)]
        WBMXL,

        [Metadata(Value = "application/vnd.criticaltools.wbs+xml", IsText = true)]
        WBS,

        [Metadata(Value = "application/vnd.wap.wbxml", IsText = true)]
        WBXML,

        [Metadata(Value = "application/vnd.ms-works", IsBinary = true)]
        WCM,

        [Metadata(Value = "application/vnd.ms-works", IsBinary = true)]
        WDB,

        [Metadata(Value = "image/vnd.ms-photo", IsBinary = true)]
        WDP,

        [Metadata(Value = "audio/webm", IsBinary = true)]
        WEBA,

        [Metadata(Value = "video/webm", IsBinary = true)]
        WEBM,

        [Metadata(Value = "image/webp", IsBinary = true)]
        WEBP,

        [Metadata(Value = "application/vnd.pmi.widget", IsBinary = true)]
        WG,

        [Metadata(Value = "application/widget", IsBinary = true)]
        WGT,

        [Metadata(Value = "application/vnd.ms-works", IsBinary = true)]
        WKS,

        [Metadata(Value = "video/x-ms-wm", IsBinary = true)]
        WM,

        [Metadata(Value = "audio/x-ms-wma", IsBinary = true)]
        WMA,

        [Metadata(Value = "application/x-ms-wmd", IsBinary = true)]
        WMD,

        [Metadata(Value = "application/x-msmetafile", IsBinary = true)]
        WMF,

        [Metadata(Value = "text/vnd.wap.wml", IsText = true)]
        WML,

        [Metadata(Value = "application/vnd.wap.wmlc", IsBinary = true)]
        WMLC,

        [Metadata(Value = "text/vnd.wap.wmlscript", IsText = true)]
        WMLS,

        [Metadata(Value = "application/vnd.wap.wmlscriptc", IsBinary = true)]
        WMLSC,

        [Metadata(Value = "video/x-ms-wmv", IsBinary = true)]
        WMV,

        [Metadata(Value = "video/x-ms-wmx", IsBinary = true)]
        WMX,

        [Metadata(Value = "application/x-msmetafile", IsBinary = true)]
        WMZ,

        [Metadata(Value = "application/font-woff", IsBinary = true)]
        WOFF,

        [Metadata(Value = "application/vnd.wordperfect", IsBinary = true)]
        WPD,

        [Metadata(Value = "application/vnd.ms-wpl", IsBinary = true)]
        WPL,

        [Metadata(Value = "application/vnd.ms-works", IsBinary = true)]
        WPS,

        [Metadata(Value = "application/vnd.wqd", IsBinary = true)]
        WQD,

        [Metadata(Value = "application/x-mswrite", IsBinary = true)]
        WRI,

        [Metadata(Value = "model/vrml", IsBinary = true)]
        WRL,

        [Metadata(Value = "application/wsdl+xml", IsText = true)]
        WSDL,

        [Metadata(Value = "application/wspolicy+xml", IsText = true)]
        WSPOLICY,

        [Metadata(Value = "application/vnd.webturbo", IsBinary = true)]
        WTB,

        [Metadata(Value = "video/x-ms-wvx", IsBinary = true)]
        WVX,

        [Metadata(Value = "application/x-authorware-bin", IsBinary = true)]
        X32,

        [Metadata(Value = "model/x3d+xml", IsText = true)]
        X3D,

        [Metadata(Value = "model/x3d+binary", IsBinary = true)]
        X3DB,

        [Metadata(Value = "model/x3d+binary", IsBinary = true)]
        X3DBZ,

        [Metadata(Value = "model/x3d+vrml", IsBinary = true)]
        X3DV,

        [Metadata(Value = "model/x3d+vrml", IsBinary = true)]
        X3DVZ,

        [Metadata(Value = "model/x3d+xml", IsText = true)]
        X3DZ,

        [Metadata(Value = "application/xaml+xml", IsText = true)]
        XAML,

        [Metadata(Value = "application/x-silverlight-app", IsBinary = true)]
        XAP,

        [Metadata(Value = "application/vnd.xara", IsBinary = true)]
        XAR,

        [Metadata(Value = "application/x-ms-xbap", IsBinary = true)]
        XBAP,

        [Metadata(Value = "application/vnd.fujixerox.docuworks.binder", IsBinary = true)]
        XBD,

        [Metadata(Value = "image/x-xbitmap", IsBinary = true)]
        XBM,

        [Metadata(Value = "application/xcap-diff+xml", IsText = true)]
        XDF,

        [Metadata(Value = "application/vnd.syncml.dm+xml", IsText = true)]
        XDM,

        [Metadata(Value = "application/vnd.adobe.xdp+xml", IsText = true)]
        XDP,

        [Metadata(Value = "application/dssc+xml", IsText = true)]
        XDSSC,

        [Metadata(Value = "application/vnd.fujixerox.docuworks", IsBinary = true)]
        XDW,

        [Metadata(Value = "application/xenc+xml", IsText = true)]
        XENC,

        [Metadata(Value = "application/patch-ops-error+xml", IsText = true)]
        XER,

        [Metadata(Value = "application/vnd.adobe.xfdf", IsBinary = true)]
        XFDF,

        [Metadata(Value = "application/vnd.xfdl", IsBinary = true)]
        XFDL,

        [Metadata(Value = "application/xhtml+xml", IsText = true)]
        XHT,

        [Metadata(Value = "application/xhtml+xml", IsText = true)]
        XHTML,

        [Metadata(Value = "application/xv+xml", IsText = true)]
        XHVML,

        [Metadata(Value = "image/vnd.xiff", IsBinary = true)]
        XIF,

        [Metadata(Value = "application/vnd.ms-excel", IsBinary = true)]
        XLA,

        [Metadata(Value = "application/vnd.ms-excel.addin.macroenabled.12", IsBinary = true)]
        XLAM,

        [Metadata(Value = "application/vnd.ms-excel", IsBinary = true)]
        XLC,

        [Metadata(Value = "application/x-xliff+xml", IsText = true)]
        XLF,

        [Metadata(Value = "application/vnd.ms-excel", IsBinary = true)]
        XLM,

        [Metadata(Value = "application/vnd.ms-excel", IsBinary = true)]
        XLS,

        [Metadata(Value = "application/vnd.ms-excel.sheet.binary.macroenabled.12", IsBinary = true)]
        XLSB,

        [Metadata(Value = "application/vnd.ms-excel.sheet.macroenabled.12", IsBinary = true)]
        XLSM,

        [Metadata(Value = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", IsBinary = true)]
        XLSX,

        [Metadata(Value = "application/vnd.ms-excel", IsBinary = true)]
        XLT,

        [Metadata(Value = "application/vnd.ms-excel.template.macroenabled.12", IsBinary = true)]
        XLTM,

        [Metadata(Value = "application/vnd.openxmlformats-officedocument.spreadsheetml.template", IsBinary = true)]
        XLTX,

        [Metadata(Value = "application/vnd.ms-excel", IsBinary = true)]
        XLW,

        [Metadata(Value = "audio/xm", IsBinary = true)]
        XM,

        [Metadata(Value = "application/xml", IsText = true)]
        XML,

        [Metadata(Value = "application/vnd.olpc-sugar", IsBinary = true)]
        XO,

        [Metadata(Value = "application/xop+xml", IsText = true)]
        XOP,

        [Metadata(Value = "application/x-xpinstall", IsBinary = true)]
        XPI,

        [Metadata(Value = "application/xproc+xml", IsText = true)]
        XPL,

        [Metadata(Value = "image/x-xpixmap", IsBinary = true)]
        XPM,

        [Metadata(Value = "application/vnd.is-xpr", IsBinary = true)]
        XPR,

        [Metadata(Value = "application/vnd.ms-xpsdocument", IsBinary = true)]
        XPS,

        [Metadata(Value = "application/vnd.intercon.formnet", IsBinary = true)]
        XPW,

        [Metadata(Value = "application/vnd.intercon.formnet", IsBinary = true)]
        XPX,

        [Metadata(Value = "application/xml", IsText = true)]
        XSL,

        [Metadata(Value = "application/xslt+xml", IsText = true)]
        XSLT,

        [Metadata(Value = "application/vnd.syncml+xml", IsText = true)]
        XSM,

        [Metadata(Value = "application/xspf+xml", IsText = true)]
        XSPF,

        [Metadata(Value = "application/vnd.mozilla.xul+xml", IsText = true)]
        XUL,

        [Metadata(Value = "application/xv+xml", IsText = true)]
        XVM,

        [Metadata(Value = "application/xv+xml", IsText = true)]
        XVML,

        [Metadata(Value = "image/x-xwindowdump", IsBinary = true)]
        XWD,

        [Metadata(Value = "chemical/x-xyz", IsBinary = true)]
        XYZ,

        [Metadata(Value = "application/x-xz", IsBinary = true)]
        XZ,

        [Metadata(Value = "application/yang", IsBinary = true)]
        YANG,

        [Metadata(Value = "application/yin+xml", IsText = true)]
        YIN,

        [Metadata(Value = "application/x-zmachine", IsBinary = true)]
        Z1,

        [Metadata(Value = "application/x-zmachine", IsBinary = true)]
        Z2,

        [Metadata(Value = "application/x-zmachine", IsBinary = true)]
        Z3,

        [Metadata(Value = "application/x-zmachine", IsBinary = true)]
        Z4,

        [Metadata(Value = "application/x-zmachine", IsBinary = true)]
        Z5,

        [Metadata(Value = "application/x-zmachine", IsBinary = true)]
        Z6,

        [Metadata(Value = "application/x-zmachine", IsBinary = true)]
        Z7,

        [Metadata(Value = "application/x-zmachine", IsBinary = true)]
        Z8,

        [Metadata(Value = "application/vnd.zzazz.deck+xml", IsText = true)]
        ZAZ,

        [Metadata(Value = "application/zip", IsBinary = true)]
        ZIP,

        [Metadata(Value = "application/vnd.zul", IsBinary = true)]
        ZIR,

        [Metadata(Value = "application/vnd.zul", IsBinary = true)]
        ZIRZ,

        [Metadata(Value = "application/vnd.handheld-entertainment+xml", IsText = true)]
        ZMM,

        [Metadata(Value = "application/octet-stream", IsBinary = true)]
        DEFAULT
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    class Metadata : Attribute
    {
        public Metadata()
        {
            this.Value = "text/plain";
            this.IsText = true;
        }

        public string Value { get; set; }
        public bool IsText { get; set; }
        public bool IsBinary
        {
            get
            {
                return !this.IsText;
            }
            set
            {
                this.IsText = !value;
            }
        }
    }


    public static class ContentTypeExtensions
    {
        private static object GetMetadata(ContentType ct)
        {
            var type = ct.GetType();
            MemberInfo[] info = type.GetMember(ct.ToString());
            if ((info != null) && (info.Length > 0))
            {
                object[] attrs = info[0].GetCustomAttributes(typeof(Metadata), false);
                if ((attrs != null) && (attrs.Length > 0))
                {
                    return attrs[0];
                }
            }
            return null;
        }

        public static string ToValue(this ContentType ct)
        {
            var metadata = GetMetadata(ct);
            return (metadata != null) ? ((Metadata)metadata).Value : ct.ToString();
        }

        public static bool IsText(this ContentType ct)
        {
            var metadata = GetMetadata(ct);
            return (metadata != null) ? ((Metadata)metadata).IsText : true;
        }

        public static bool IsBinary(this ContentType ct)
        {
            var metadata = GetMetadata(ct);
            return (metadata != null) ? ((Metadata)metadata).IsBinary : false;
        }
    }
}