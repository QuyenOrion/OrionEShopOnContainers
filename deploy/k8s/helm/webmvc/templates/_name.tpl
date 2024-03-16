{{- define "secret-name" -}}
{{- $name := include "webmvc.fullname" .}}
{{- printf "%s-secret" $name -}}
{{- end -}}

{{- define "protocol" -}}
{{- $protocol := .Values.info.tls.enabled | ternary "https" "http" }}
{{- printf "%s" $protocol -}}
{{- end -}}